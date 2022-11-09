using infrastructure.sqlserver.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Data.SqlClient;
using healthspanmd.core.CQRS.Users;
using infrastructure.sqlserver.CQRS.Users;
using healthspanmd.core.Services.ExceptionReporting;
using System.IdentityModel.Tokens.Jwt;
using healthspanmd.core.CQRS.Metrics;
using infrastructure.sqlserver.CQRS.Metrics;
using healthspanmd.core.CQRS.MetricTrackingEntries;
using infrastructure.sqlserver.CQRS.MetricTrackingEntries;
using healthspanmd.core.Services.SMS;
using infrastructure.clicksend;
using healthspanmd.core.CQRS.Notifications;
using infrastructure.sqlserver.CQRS.Notifications;
using healthspanmd.core.Services.Authorization;
using healthspanmd.web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using infrastructure.gmail;
using healthspanmd.core.Services.FileSystem;
using infrastructure.filesystem;
using healthspanmd.core.CQRS.Content;
using infrastructure.sqlserver.CQRS.Content;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using healthspanmd.core.Services.Messaging;
using infrastructure.notifications;

namespace healthspanmd.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddMvc()
                    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddKendo();

            services.Configure<EmailSenderOptions>(Configuration.GetSection("EmailSettings"));

            // configure DbContext
            var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<HealthSpanMdDbContext>(options =>
                options.UseSqlServer(builder.ConnectionString));

            services.AddTransient<IUserCommands, UserCommands>();
            services.AddTransient<IUserQueries, UserQueries>();
            services.AddTransient<IExceptionReporter, ExceptionReporter>();
            services.AddTransient<IMetricQueries, MetricQueries>();
            services.AddTransient<IMetricCommands, MetricCommands>();
            services.AddTransient<IMetricTrackingEntryCommands, MetricTrackingEntryCommands>();
            services.AddTransient<IMetricTrackingEntryQueries, MetricTrackingEntryQueries>();
            services.AddTransient<ISMSProvider, SMSProvider>();
            services.AddTransient<IEmailSender, GmailSender>();
            services.AddTransient<INotificationQueries, NotificationQueries>();
            services.AddTransient<IRequestParser, RequestParser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IFileSystemManager, FileSystemManager>();
            services.AddTransient<IContentCommands, ContentCommands>();
            services.AddTransient<IContentQueries, ContentQueries>();
            services.AddTransient<INotificationSender, NotificationSender>();
            services.AddTransient<IHtmlEmailSender, HtmlEmailSender>();
            services.AddTransient<INotificationCommands, NotificationCommands>();
            
            // Authorization Services
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanAccessClientList",
                    policy => policy.Requirements.Add(new CanAccessClientListRequirement()));
                options.AddPolicy("CanEditMetrics",
                    policy => policy.Requirements.Add(new CanEditMetricsRequirement()));
                options.AddPolicy("CanViewCoachDashboard",
                    policy => policy.Requirements.Add(new CanViewCoachDashboardRequirement()));
                options.AddPolicy("CanViewUserProgress",
                    policy => policy.Requirements.Add(new CanViewUserProgressRequirement()));             
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();



            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(60);
                    options.SlidingExpiration = true;
                    options.Events.OnSigningIn = (context) =>
                    {
                        context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(60);
                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                })
                //.AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.UsePkce = false;
                    options.Authority = Configuration.GetSection("OpenIdConnectSettings").GetValue<string>("Authority");
                    //IdentityModelEventSource.ShowPII = true;                    
                    options.RequireHttpsMetadata = Configuration.GetSection("OpenIdConnectSettings").GetValue<bool>("RequireHttpsMetadata");
                    //options.UseTokenLifetime = true;

                    options.ClientId = Configuration.GetSection("OpenIdConnectSettings").GetValue<string>("ClientId");
                    options.ClientSecret = Configuration.GetSection("OpenIdConnectSettings").GetValue<string>("ClientSecret");
                    options.ResponseType = "code id_token";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.SaveTokens = true;

                    //options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
                    //options.Scope.Add("roles");

                    options.SignedOutRedirectUri = "/home";

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = System.Security.Claims.ClaimTypes.Role
                    };


                });

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

            app.UseAuthentication();
            app.UseAuthorization();

            var contentSecurityPolicy = "default-src 'self' https://localhost https://localhost:44362 ;" +
                                        "script-src 'self' https://localhost https://localhost:44362 ;" + 
                                        "style-src 'self';" + 
                                        "img-src 'self';";

            app.Use(async (context, next) => 
            {
                context.Response.Headers.Add("Content-Security_Policy", contentSecurityPolicy);
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
