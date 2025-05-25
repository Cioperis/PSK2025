using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Messaging.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ApiService.Data;
using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Services
{
    public class AutoMessageService : IAutoMessageService
    {
        private readonly AppDbContext _dbContext;
        private readonly IRabbitMQueue _rabbit;

        public AutoMessageService(AppDbContext dbContext, IRabbitMQueue rabbit)
        {
            _dbContext = dbContext;
            _rabbit = rabbit;
        }

        public async Task<bool> EnqueueRandomPositiveMessageAsync(string userEmail)
        {
            var message = await _dbContext.PositiveMessage
                .Where(p => p.IsEnabled)
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefaultAsync();

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
    }
}
