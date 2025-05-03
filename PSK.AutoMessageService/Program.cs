var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
