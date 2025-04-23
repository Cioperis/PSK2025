using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PSK_ApiService>("api")
       .WithExternalHttpEndpoints();

builder.AddProject<Projects.PSK_MigrationService>("migrations");

builder.AddNpmApp("reactvite", "../PSK.Web");

builder.Build().Run();
