//using PSK.AutoMessageService.Services;
//using RabbitMQ.Client.Events;

//namespace PSK.AutoMessageService.Workers
//{
//    public class AutoMessageWorker : BackgroundService
//    {
//        private readonly ILogger<AutoMessageWorker> _logger;
//        private readonly NotificationService _notificationService;
//        private readonly IRabbitMQConnection _connection;

//        public AutoMessageWorker(ILogger<AutoMessageWorker> logger, NotificationService notificationService, IRabbitMQConnection connection)
//        {
//            _logger = logger;
//            _notificationService = notificationService;
//            _connection = connection;
//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var channel = _connection.CreateChannel();
//            channel.QueueDeclare("positive_messages", durable: false, exclusive: false, autoDelete: false, arguments: null);

//            var consumer = new EventingBasicConsumer(channel);
//            consumer.Received += async (model, ea) =>
//            {
//                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
//                _logger.LogInformation("Received: {Message}", message);
//                await _notificationService.SendNotificationAsync(message);
//            };

//            channel.BasicConsume(queue: "positive_messages", autoAck: true, consumer: consumer);
//            return Task.CompletedTask;
//        }
//    }

//}
