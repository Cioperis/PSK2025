using Serilog;
using Aspire.Hosting;

SerilogExtensions.ConfigureAppHostSerilog();

try
{
    Log.Information("Starting PSK AppHost");

    var builder = (DistributedApplicationBuilder)DistributedApplication.CreateBuilder(args);

    builder.AddPskServices();

    Log.Information("Building and running the application");
    builder.Build().Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
