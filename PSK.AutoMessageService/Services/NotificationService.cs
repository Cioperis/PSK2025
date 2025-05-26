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
                From = new MailAddress(_emailSettings.FromMail, "PSK Team"),
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

            var subject = "Welcome to PSK!";
            var body = GetWelcomeEmailBody(name, timestamp);

            return SendEmailAsync(email, subject, body, isHtml: true);
        }

        private string GetWelcomeEmailBody(string name, DateTime timestamp)
        {
            return $@"  
    <!DOCTYPE html>  
    <html lang='en'>  
    <head>  
     <meta charset='UTF-8'>  
     <meta name='viewport' content='width=device-width, initial-scale=1.0'>  
     <title>Welcome to PSK!</title>  
    </head>  
    <body style='font-family: Arial, sans-serif; background-color: #f6f8fa; margin:0; padding:0;'>  
     <table role='presentation' width='100%' cellspacing='0' cellpadding='0' border='0' style='background:#f6f8fa;padding:24px 0;'>  
       <tr>  
         <td align='center'>  
           <table width='600' style='background:#fff; border-radius:8px; box-shadow:0 2px 12px rgba(0,0,0,0.07); overflow:hidden;'>  
             <tr>  
               <td style='padding:32px 32px 8px 32px; text-align:center;'>  
                 <h1 style='margin:0;color:#2e86de;'>Welcome to PSK, {name}!</h1>  
               </td>  
             </tr>  
             <tr>  
               <td style='padding:0 32px 24px 32px; color:#444; text-align:center;'>  
                 <p style='font-size:16px; margin:24px 0 8px;'>Your account was created on <b>{timestamp:f}</b>.</p>  
                 <p style='font-size:16px; margin:8px 0;'>Thank you for joining our platform.<br>We’re excited to have you!</p>  
                 <p style='font-size:16px; margin:16px 0 32px;'>If you have any questions, just reply to this email or contact our support team.</p>  
                 <a href='https://yourappdomain.com/login' style='background:#2e86de;color:#fff;padding:12px 24px;text-decoration:none;border-radius:5px;font-weight:bold;display:inline-block;margin-bottom:20px;'>Go to your dashboard</a>  
                 <p style='font-size:14px; color:#888; margin-top:40px;'>— The PSK Team</p>  
               </td>  
             </tr>  
           </table>  
         </td>  
       </tr>  
     </table>  
    </body>  
    </html>";
        }
    }
}
