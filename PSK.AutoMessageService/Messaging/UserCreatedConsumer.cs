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
#pragma warning disable IDE1006 // Naming Styles
        public required string email { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public required string name { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public DateTime timestamp { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
