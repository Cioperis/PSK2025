using PSK.ServiceDefaults.DTOs;
using RabbitMQ.Client;
using Serilog;

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
            if (string.IsNullOrWhiteSpace(message.Email) || string.IsNullOrWhiteSpace(message.Content))
            {
                Log.Warning("Received invalid AutoMessageDTO: {@Message}", message);
                return;
            }

            try
            {
                var subject = "Your Daily Positive Message";
                var body = GetPositiveMessageEmailBody(message.Content);

                await _notificationService.SendEmailAsync(
                    message.Email,
                    subject,
                    body,
                    isHtml: true
                );

                Log.Information("Positive message sent to {Email}", message.Email);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send positive message to {Email}", message.Email);
            }
        }

        private string GetPositiveMessageEmailBody(string content)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <title>Your Daily Positive Message</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f9fafb; padding:0; margin:0;'>
  <table align='center' width='100%' cellpadding='0' cellspacing='0' style='max-width:600px; margin:auto; background:#fff; border-radius:8px; box-shadow:0 2px 12px rgba(0,0,0,0.07);'>
    <tr>
      <td style='padding:40px 40px 24px 40px; text-align:center;'>
        <h2 style='color:#27ae60;'>Your Daily Positive Message</h2>
        <p style='font-size:1.2rem; color:#444; margin:24px 0 24px 0;'><b>{content}</b></p>
        <p style='font-size:14px; color:#888;'>Have a wonderful day!<br/>— The PSK Team</p>
      </td>
    </tr>
  </table>
</body>
</html>";
        }
    }
}
