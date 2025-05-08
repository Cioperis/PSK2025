using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PSK.AutoMessageService.Messaging
{
    public class NotificationService
    {
        private readonly EmailSettings _emailSettings;

        public NotificationService(IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions.Value;
            Log.Information("NotificationService initialized with provided email settings.");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            Log.Information("Preparing to send email to {ToEmail} with subject: {Subject}", toEmail, subject);

            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromMail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(new MailAddress(toEmail));

            using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_emailSettings.FromMail, _emailSettings.FromPassword),
                EnableSsl = true,
            };

            try
            {
                await smtpClient.SendMailAsync(message);
                Log.Information("Email successfully sent to {ToEmail}.", toEmail);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to send email to {ToEmail}.", toEmail);
                throw;
            }
        }

        public Task SendUserCreatedNotificationAsync(string email, string name, DateTime timestamp)
        {
            Log.Information("Sending user creation notification to {Email} for user {Name}.", email, name);

            var subject = "Welcome to our platform!";
            var body = $@"  
                   <html>  
                   <body>  
                       <h2>Welcome {name}!</h2>  
                       <p>Your account was created on {timestamp:f}.</p>  
                       <p>Thank you for registering with our platform. We're excited to have you on board!</p>  
                       <p>If you have any questions, please contact our support team.</p>  
                       <p>Best regards,<br>The Team</p>  
                   </body>  
                   </html>";

            return SendEmailAsync(email, subject, body, isHtml: true);
        }
    }
}
