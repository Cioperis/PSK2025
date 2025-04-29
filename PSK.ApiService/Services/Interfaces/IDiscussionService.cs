using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Services.Interfaces;

public interface IDiscussionService
{
    Task<DiscussionDTO> CreateDiscussionAsync(DiscussionDTO discussion);
    Task<IEnumerable<DiscussionDTO>> GetAllDiscussionsAsync();
    Task<DiscussionDTO?> GetDiscussionAsync(Guid discussionId);
    Task DeleteDiscussionAsync(Guid discussionId);
}