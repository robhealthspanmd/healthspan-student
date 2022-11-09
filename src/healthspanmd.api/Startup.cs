using healthspanmd.core.CQRS.ActiveMetrics;
using healthspanmd.core.CQRS.Metrics;
using healthspanmd.core.CQRS.MetricTrackingEntries;
//using healthspanmd.core.CQRS.MetricTypes;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.ExceptionReporting;
using healthspanmd.core.Services.HealthAgeCalculator_v2;
using infrastructure.sqlserver.CQRS.ActiveMetrics;
using infrastructure.sqlserver.CQRS.Metrics;
using infrastructure.sqlserver.CQRS.MetricTrackingEntries;
//using infrastructure.sqlserver.CQRS.MetricTypes;
using infrastructure.sqlserver.CQRS.Users;
using infrastructure.sqlserver.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            // configure DbContext
            var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<HealthSpanMdDbContext>(options =>
                options.UseSqlServer(builder.ConnectionString));



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "healthspanmd.api", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "ApiKey",
                    Description = "My description"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                        },
                        new string[] { }
                    }
                });
            });


            services.AddTransient<IHealthAgeCalculator, HealthAgeCalculator>();
            services.AddTransient<IUserCommands, UserCommands>();
            services.AddTransient<IUserQueries, UserQueries>();
            //services.AddTransient<IMetricTypeCommands, MetricTypeCommands>();
            //services.AddTransient<IMetricTypeQueries, MetricTypeQueries>();
            services.AddTransient<IMetricCommands, MetricCommands>();
            services.AddTransient<IMetricQueries, MetricQueries>();
            services.AddTransient<IActiveMetricCommands, ActiveMetricCommands>();
            services.AddTransient<IExceptionReporter, ExceptionReporter>();
            services.AddTransient<IMetricTrackingEntryCommands, MetricTrackingEntryCommands>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "healthspanmd.api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
