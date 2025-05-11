using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PSK.ApiService.Authentication;
using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Repositories;
using PSK.ApiService.Services.Interfaces;
using PSK.ApiService.Services;
using PSK.ApiService.Chatting;
using Serilog;
using Serilog.Events;
using PSK.ApiService.Messaging.Interfaces;
using PSK.ApiService.Messaging;

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
    builder.Services.AddScoped<IAutoMessageRepository, AutoMessageRepository>();
    builder.Services.AddScoped<IAutoMessageService, AutoMessageService>();
    builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();
    builder.Services.AddScoped<IDiscussionService, DiscussionService>();
    builder.Services.AddScoped<ICommentService, CommentService>();

    builder.Services.Configure<JwtSettings>(options =>
    {
        options.Key = builder.Configuration["JwtSettings:Key"]
                      ?? throw new InvalidOperationException("JWT Key not found in environment or user-secrets.");
        options.Issuer = "PSK.ApiService";
        options.Audience = "PSK.Client";
        options.ExpiresInMinutes = 60;
    });

    builder.Services.AddScoped<ITokenService, TokenService>();

    var jwtKey = builder.Configuration["JwtSettings:Key"]
                 ?? throw new InvalidOperationException("JWT Key not found.");
    var jwtIssuer = "PSK.ApiService";
    var jwtAudience = "PSK.Client";

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter ‘Bearer <token>’"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
    builder.Services.AddSignalR();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost5173", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    builder.AddRabbitMQClient("rabbitmq");
    builder.Services.AddSingleton<IRabbitMQueue, RabbitMQueue>();

    var app = builder.Build();

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