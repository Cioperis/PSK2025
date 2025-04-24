using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PSK.ApiService.Data;
using PSK.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ApiDbInitializer>();

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb"), npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("PSK.MigrationService");
    }));
builder.EnrichNpgsqlDbContext<AppDbContext>(settings =>
    // Disable Aspire default retries if needed
    settings.DisableRetry = true);

Console.WriteLine("🔐 Connection: " + builder.Configuration.GetConnectionString("postgresdb"));

var app = builder.Build();
app.Run();
