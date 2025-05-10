using RabbitMQ.Client;

namespace PSK.AutoMessageService.Messaging
{
    public class UserCreatedConsumer : RabbitMqConsumerBase<UserCreatedMessage>
    {
        private readonly NotificationService _notificationService;

        public UserCreatedConsumer(IConnection connection, NotificationService notificationService)
            : base(connection, "user.created")
        {
            _notificationService = notificationService;
        }

        protected override async Task HandleMessageAsync(UserCreatedMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.email)) return;

            await _notificationService.SendUserCreatedNotificationAsync(
                message.email, message.name, message.timestamp);
            Console.WriteLine($"UserCreated email sent to {message.email}");
        }
    }

    public class UserCreatedMessage
    {
        public string email { get; set; }
        public string name { get; set; }
        public DateTime timestamp { get; set; }
    }
}
