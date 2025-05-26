using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;
using PSK.ServiceDefaults.Schema;

namespace PSK.ApiService.Services.Interfaces
{
    public interface IAutoMessageService
    {
        Task<bool> EnqueueRandomPositiveMessageAsync(string userEmail);
        Task<ScheduleUserMessageResult> ScheduleUserCustomMessageAsync(Guid userId, UserMessageRequest request);
        Task SendUserCustomMessage(Guid userMessageId);
        Task<IEnumerable<UserMessageDto>> GetUsersCustomMessages(Guid userId);
        Task<UserMessageDto?> GetCustomMessage(Guid messageId);
        Task<bool> DeleteUserMessage(Guid messageId, Guid userId);
    }
}
