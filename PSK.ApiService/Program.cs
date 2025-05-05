using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Repositories;
using PSK.ApiService.Services.Interfaces;
using PSK.ApiService.Services;
using PSK.ApiService.Chatting;
using Serilog;
using Serilog.Events;

// ./bin/debug/net9.0/PSK.ApiService
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
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();
    builder.Services.AddScoped<IDiscussionService, DiscussionService>();
    builder.Services.AddScoped<ICommentService, CommentService>();
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSignalR();
    
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy( policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseCors();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
    app.MapHub<ChatHub>("/chatHub");

    app.UseHttpsRedirection();
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
