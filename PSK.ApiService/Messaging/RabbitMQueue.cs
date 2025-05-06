using PSK.ApiService.Messaging.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace PSK.ApiService.Messaging
{
    public class RabbitMQueue : IRabbitMQueue, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private bool _disposed;

        public RabbitMQueue(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public void PublishMessage(string queue, string message, string exchange = "", IBasicProperties properties = null)
        {
            // Ensure queue exists
            _channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Publish message
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: queue,
                basicProperties: properties,
                body: body);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _channel?.Close();
                    _channel?.Dispose();
                    _connection?.Close();
                    _connection?.Dispose();
                }

                _disposed = true;
            }
        }
    }

}
