using healthspanmd.core.CQRS.Notifications;
using healthspanmd.core.CQRS.Users;
using healthspanmd.core.Services.Messaging;
using healthspanmd.core.Services.ExceptionReporting;
using healthspanmd.core.Services.SMS;
using infrastructure.clicksend;
using infrastructure.gmail;
using infrastructure.sqlserver.CQRS.Notifications;
using infrastructure.sqlserver.CQRS.Users;
using infrastructure.sqlserver.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using schedulednotifications.service;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    //.UseSerilog((hostContext, loggerConfiguration) =>
    //{
    //    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
    //})
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices((hostContext, services) =>
    {
        // configure DbContext
        var builder = new SqlConnectionStringBuilder(hostContext.Configuration.GetConnectionString("DefaultConnection"));
        services.AddDbContext<HealthSpanMdDbContext>(options =>
            options.UseSqlServer(builder.ConnectionString));

        //services.AddLogging();

        services.AddHostedService<Worker>();
        services.AddTransient<ISMSProvider, SMSProvider>();
        services.AddTransient<INotificationCommands, NotificationCommands>();
        services.AddTransient<INotificationQueries, NotificationQueries>();
        services.AddTransient<IUserQueries, UserQueries>();
        services.AddTransient<IExceptionReporter, ExceptionReporter>();
        services.AddTransient<IEmailSender, GmailSender>();
    })
    
    .Build();

await host.RunAsync();
