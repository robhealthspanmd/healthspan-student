using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using healthspan.sso.web.Data;
using healthspan.sso.web.Models;
using healthspan.sso.web.Services;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.ExceptionReporting;
using infrastructure.sqlserver.CQRS.Users;
using infrastructure.sqlserver.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Security.Cryptography;
using infrastructure.gmail;
using healthspanmd.core.Services.Messaging;

namespace healthspan.sso.web
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.File(configuration.GetValue<string>("LogFilePath"), rollingInterval: RollingInterval.Day)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .WriteTo.AzureApp()
                .CreateLogger();

            
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // configure DbContext
            var sqlcsbuilder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<HealthSpanMdDbContext>(options =>
                options.UseSqlServer(sqlcsbuilder.ConnectionString));


            services.AddTransient<IEmailSender, GmailSender>();
            services.AddTransient<IUserCommands, UserCommands>();
            services.AddTransient<IUserQueries, UserQueries>();
            services.AddTransient<IExceptionReporter, ExceptionReporter>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/identity/account/login";
                options.UserInteraction.LogoutUrl = "/identity/account/logout";
                options.Endpoints.EnableTokenEndpoint = true;
                options.Endpoints.EnableDiscoveryEndpoint = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients(Configuration))
                .AddAspNetIdentity<ApplicationUser>()

                .AddProfileService<ProfileService>();

            //if (Environment.IsDevelopment())
            //{
                builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    var creds = GetIDS4CertCreds();
            //    Log.Information("creds generated");

            //    builder.AddSigningCredential(creds.key, creds.algorithm);
            //    Log.Information("signing credentials added");
            //}


            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        // register your IdentityServer with Google at https://console.developers.google.com
            //        // enable the Google+ API
            //        // set the redirect URI to https://localhost:5001/signin-google
            //        options.ClientId = "copy client ID from Google here";
            //        options.ClientSecret = "copy client secret from Google here";
            //    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }




        private SigningCredentials GetIDS4CertCreds()
        {
            var keyClient = new KeyClient(
            new Uri("https://healthspanmdkeyvault.vault.azure.net/"), // Vault URI, e.g: https://scottbrady91-test.vault.azure.net/
            new ClientSecretCredential(
                tenantId: "2c17d018-77a5-4cc2-b044-8147fd849475",
                clientId: "79ed9dc4-003d-4a2f-bd64-be659b1292ab",
                clientSecret: "LFz7Q~CeNOw1e_igoGLFbiJEhdV8Kjs2NhEcU"));

            Response<KeyVaultKey> response = keyClient.GetKey("IdentityServerSigningECKey"); // key name, e.g: IdentityServerSigningKey



            AsymmetricSecurityKey key;
            string algorithm;

            if (response.Value.KeyType == KeyType.Ec)
            {
                ECDsa ecDsa = response.Value.Key.ToECDsa();
                key = new ECDsaSecurityKey(ecDsa) { KeyId = response.Value.Properties.Version };

                // parse from curve
                if (response.Value.Key.CurveName == KeyCurveName.P256) algorithm = "ES256";
                else if (response.Value.Key.CurveName == KeyCurveName.P384) algorithm = "ES384";
                else if (response.Value.Key.CurveName == KeyCurveName.P521) algorithm = "ES521";
                else throw new NotSupportedException();
            }
            else if (response.Value.KeyType == KeyType.Rsa)
            {
                RSA rsa = response.Value.Key.ToRSA();
                key = new RsaSecurityKey(rsa) { KeyId = response.Value.Properties.Version };

                // you define
                algorithm = "PS256";
            }
            else
            {
                throw new NotSupportedException();
            }

            return new SigningCredentials
            {
                key = (ECDsaSecurityKey)key,
                algorithm = algorithm,
            };
        }

        public class SigningCredentials
        {
            public ECDsaSecurityKey key { get; set; }
            public string algorithm { get; set; }
        }








        //private X509Certificate2 GetIdentityServerCertificate(IServiceCollection services)
        //{
        //    var clientId = Configuration.GetSection("AzureKeyVaultClient:ClientId").Value;
        //    var clientSecret = Configuration.GetSection("AzureKeyVaultClient:ClientSecret").Value;
        //    var secretIdentifier = Configuration.GetSection("AzureKeyVaultClient:SecretIdentifier").Value;

        //    var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (authority, resource, scope) =>
        //    {
        //        var authContext = new AuthenticationContext(authority);
        //        ClientCredential clientCreds = new ClientCredential(clientId, clientSecret);

        //        AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCreds);

        //        if (result == null)
        //        {
        //            throw new InvalidOperationException("Failed to obtain the JWT token");
        //        }

        //        return result.AccessToken;
        //    }));

        //    var pfxSecret = keyVaultClient.GetSecretAsync(secretIdentifier).Result;




        //    var pfxBytes = Convert.FromBase64String(pfxSecret.Value);
        //    var certificate = new X509Certificate2(pfxBytes);
        //    return certificate;
        //}





    }
}
