using PSK.ServiceDefaults.DTOs;
using RabbitMQ.Client;

namespace PSK.AutoMessageService.Messaging
{
    public class AutoMessageConsumer : RabbitMqConsumerBase<AutoMessageDTO>
    {
        private readonly NotificationService _notificationService;

        public AutoMessageConsumer(IConnection connection, NotificationService service)
            : base(connection, "auto-message.send")
        {
            _notificationService = service;
        }

        protected override async Task HandleMessageAsync(AutoMessageDTO message)
        {
            await _notificationService.SendEmailAsync(
                message.Email,
                "Your Daily Positive Message",
                message.Content,
                isHtml: false);
        }
    }
}
