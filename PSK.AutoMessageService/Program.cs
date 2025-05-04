using PSK.AutoMessageService;
using PSK.AutoMessageService.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddSingleton<NotificationService>();
builder.Services.AddHostedService<UserCreatedConsumer>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
