using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PSK.AutoMessageService.Messaging
{
    public abstract class RabbitMqConsumerBase<TMessage> : BackgroundService
    {
        private readonly IModel _channel;
        private readonly string _queueName;

        protected RabbitMqConsumerBase(IConnection connection, string queueName)
        {
            _channel = connection.CreateModel();
            _queueName = queueName;

            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var deserialized = JsonSerializer.Deserialize<TMessage>(message,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (deserialized != null)
                        await HandleMessageAsync(deserialized);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        protected abstract Task HandleMessageAsync(TMessage message);

        public override void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
