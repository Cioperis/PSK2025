using Hangfire;
using Hangfire.PostgreSql;

public static class HangfireExtensions
{
    public static IServiceCollection AddPskHangfire(this IServiceCollection services, IConfiguration config)
    {
        services.AddHangfire(hfConfig =>
            hfConfig.UsePostgreSqlStorage(config.GetConnectionString("postgresdb")));
        services.AddHangfireServer();
        return services;
    }
}
