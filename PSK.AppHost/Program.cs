using Aspire.Hosting;
using Aspire.Hosting.Postgres;
using Serilog;
using Serilog.Events;
using System;

// ./bin/debug/net9.0/PSK.AppHost
// idea is to have logs in the same directory for everyone
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
    Log.Information("Starting PSK AppHost");

    var builder = DistributedApplication.CreateBuilder(args);

    // Add Serilog to the builder if supported
    var logging = builder.Services.AddSerilog();

    var rabbitMQ = builder.AddRabbitMQ("rabbitmq")
        .WithManagementPlugin(port: 15672);

    var redis = builder.AddRedis("redis");

    var postgres = builder.AddPostgres("postgres")
        .WithDataVolume()
        .WithPgWeb();

    var postgresdb = postgres.AddDatabase("postgresdb");

    builder.AddProject<Projects.PSK_MigrationService>("migrations")
        .WithReference(postgresdb);

    builder.AddProject<Projects.PSK_ApiService>("api")
        .WithReference(postgresdb)
        .WithReference(rabbitMQ)
        .WithReference(redis)
        .WaitFor(rabbitMQ);

    builder.AddProject<Projects.PSK_AutoMessageService>("autoMessages")
        .WithReference(rabbitMQ)
        .WaitFor(rabbitMQ);

    builder.AddNpmApp("reactvite", "../PSK.Web");

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
