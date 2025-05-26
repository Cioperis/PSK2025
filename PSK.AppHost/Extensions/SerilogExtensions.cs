using Serilog;
using Serilog.Events;

public static class SerilogExtensions
{
    public static void ConfigureAppHostSerilog()
    {
        string basePath = AppContext.BaseDirectory;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File($"{basePath}/Logging/Logs.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Serilog logger configured for AppHost");
    }
}