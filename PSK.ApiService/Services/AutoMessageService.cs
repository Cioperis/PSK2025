using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Messaging.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ApiService.Data;
using PSK.ServiceDefaults.DTOs;
using Hangfire;
using PSK.ServiceDefaults.Schema;

namespace PSK.ApiService.Services
{
    using System.Text.Json;
    using PSK.ApiService.Messaging.Interfaces;
    using PSK.ApiService.Services.Interfaces;
    using PSK.ServiceDefaults.DTOs;
    using Hangfire;
    using PSK.ServiceDefaults.Schema;
    using PSK.ServiceDefaults.Models;
    using PSK.ApiService.Repositories.Interfaces;

    public class AutoMessageService : IAutoMessageService
    {
        private readonly IAutoMessageRepository _autoMessageRepo;
        private readonly IUserMessageRepository _userMessageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IRabbitMQueue _rabbit;

        public AutoMessageService(
            IAutoMessageRepository autoMessageRepo,
            IUserMessageRepository userMessageRepo,
            IUserRepository userRepo,
            IRabbitMQueue rabbit)
        {
            _autoMessageRepo = autoMessageRepo;
            _userMessageRepo = userMessageRepo;
            _userRepo = userRepo;
            _rabbit = rabbit;
        }

        public async Task<bool> EnqueueRandomPositiveMessageAsync(string userEmail)
        {
            var message = await _autoMessageRepo.GetRandomActiveMessageAsync();

            if (message == null)
                return false;

            var payload = new AutoMessageDTO
            {
                Email = userEmail,
                Content = message.Content
            };

            _rabbit.PublishMessage("auto-message.send", JsonSerializer.Serialize(payload));
            return true;
        }

        public async Task<ScheduleUserMessageResult> ScheduleUserCustomMessageAsync(Guid userId, UserMessageRequest request)
        {
            var user = await _userRepo.GetByIdAsync(userId); 
            if (user == null)
                return new ScheduleUserMessageResult { Success = false, ErrorMessage = "User not found." };

            var userMessage = new UserMessage
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id,
                Content = request.Content,
                SendAt = request.SendAt,
                IsRecurring = request.IsRecurring,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userMessageRepo.AddAsync(userMessage);

            if (request.IsRecurring)
            {
                var jobId = $"recurring-user-msg-{userMessage.Id}";
                Hangfire.RecurringJob.AddOrUpdate<IAutoMessageService>(
                    jobId,
                    svc => svc.SendUserCustomMessage(userMessage.Id),
                    Cron.Daily(request.SendAt.Hour)
                );
            }
            else
            {
                Hangfire.BackgroundJob.Schedule<IAutoMessageService>(
                    svc => svc.SendUserCustomMessage(userMessage.Id),
                    request.SendAt
                );
            }

            return new ScheduleUserMessageResult { Success = true, MessageId = userMessage.Id, ErrorMessage = "OK" };
        }

        public async Task SendUserCustomMessage(Guid userMessageId)
        {
            var msg = await _userMessageRepo.GetWithUserAsync(userMessageId);
            if (msg == null || !msg.IsEnabled) return;

            var payload = new AutoMessageDTO
            {
                Email = msg.User.Email,    
                Content = msg.Content
            };

            _rabbit.PublishMessage("auto-message.send", JsonSerializer.Serialize(payload));

            if (!msg.IsRecurring)
            {
                msg.IsEnabled = false;
                await _userMessageRepo.UpdateAsync(msg);
            }
        }

    }
}
