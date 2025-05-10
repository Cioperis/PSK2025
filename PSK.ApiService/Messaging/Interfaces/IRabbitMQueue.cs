using RabbitMQ.Client;

namespace PSK.ApiService.Messaging.Interfaces
{
    public interface IRabbitMQueue
    {
        void PublishMessage(string queue, string message, string exchange = "", IBasicProperties properties = null);
        // Add other methods as needed (subscribe, etc.)
    }

}
