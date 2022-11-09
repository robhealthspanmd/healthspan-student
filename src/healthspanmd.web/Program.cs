using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace healthspanmd.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                })
                .ConfigureServices(services =>
                {
                    services.Configure<AzureFileLoggerOptions>(options =>
                    {
                        options.FileName = "first-azure-log";
                        options.FileSizeLimit = 50 * 1024;
                        options.RetainedFileCountLimit = 10;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
               //.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
               //         .ReadFrom.Configuration(hostingContext.Configuration)
               //         .WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = "xxxxxxxxx" }, TelemetryConverter.Traces)
               //  )
                ;
    }
}
