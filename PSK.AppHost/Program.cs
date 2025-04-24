using Aspire.Hosting;
using Aspire.Hosting.Postgres;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgWeb();

var postgresdb = postgres.AddDatabase("postgresdb");

builder.AddProject<Projects.PSK_MigrationService>("migrations")
    .WithReference(postgresdb);

builder.AddProject<Projects.PSK_ApiService>("api")
    .WithReference(postgresdb);

builder.AddNpmApp("reactvite", "../PSK.Web");

builder.Build().Run();
