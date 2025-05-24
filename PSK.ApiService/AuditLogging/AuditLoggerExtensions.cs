namespace PSK.ApiService.AuditLogging;

public static class AuditLoggerExtensions
{
    public static IServiceCollection AddAuditLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<SerilogAuditLogger>();
        services.AddTransient<MongoDbAuditLogger>();
        
        services.AddScoped<IAuditLogger>(provider => 
            ResolveLoggerStrategy(provider, configuration));
        
        return services;
    }

    private static IAuditLogger ResolveLoggerStrategy(IServiceProvider provider, IConfiguration configuration)
    {
        var loggerType = configuration["AuditLog:LoggerType"] ?? "Serilog";
        
        return loggerType switch
        {
            "Serilog" => provider.GetRequiredService<SerilogAuditLogger>(),
            "MongoDB" => provider.GetRequiredService<MongoDbAuditLogger>(),
            _ => throw new InvalidOperationException(
                $"Unknown logger type: {loggerType}. Check appsetings.json LoggerType. Availalbe types: Serilog, MongoDB.")
        };
    }
}