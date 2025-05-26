using PSK.AutoMessageService;
using PSK.AutoMessageService.Messaging;
using Serilog;
using Serilog.Events;

// ./bin/debug/net9.0/PSK.AutoMessageService
string basePath = AppContext.BaseDirectory;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File($"{basePath}/Logging/Logs.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.AddServiceDefaults();
    builder.AddRabbitMQClient("rabbitmq");

    builder.Services.Configure<EmailSettings>(
        builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddSingleton<NotificationService>();
    builder.Services.AddHostedService<UserCreatedConsumer>();
    builder.Services.AddHostedService<AutoMessageConsumer>();

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}
