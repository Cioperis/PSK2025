namespace PSK.AutoMessageService.Services
{
    public class NotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendNotificationAsync(string message)
        {
            _logger.LogInformation("Sending notification: {Message}", message);
            return Task.CompletedTask;
        }
    }

}
