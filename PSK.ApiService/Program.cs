using Serilog;
using PSK.ApiService.Extensions;
using Hangfire;
using PSK.ApiService.Chatting;
using PSK.ApiService.Data;
using Serilog.Events;

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

    builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "postgresdb");
    builder.Services
        .AddPskApiServices()
        .AddPskJwtAuthentication(builder.Configuration)
        .AddPskSwagger()
        .AddPskHangfire(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddSignalR();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost5173", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    builder.AddRabbitMQClient("rabbitmq");
    builder.AddRedisClient("redis");

    var app = builder.Build();

    app.UseHangfireDashboard();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost5173");
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
    app.MapHub<ChatHub>("/chatHub");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

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
