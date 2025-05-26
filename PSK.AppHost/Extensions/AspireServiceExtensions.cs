using Aspire.Hosting;

public static class AspireServiceExtensions
{
    public static DistributedApplicationBuilder AddPskServices(this DistributedApplicationBuilder builder)
    {
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

        return builder;
    }
}
