using Microsoft.Extensions.Hosting.Schema;
using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Services.Interfaces;

public interface IDiscussionService
{
    Task<DiscussionDTO> CreateDiscussionAsync(CreateDiscussionSchema discussion);
    Task<DiscussionDTO> UpdateDiscussionAsync(DiscussionDTO discussion);
    Task<IEnumerable<DiscussionDTO>> GetAllDiscussionsAsync();
    Task<DiscussionDTO?> GetDiscussionAsync(Guid discussionId);
    Task<bool> DeleteDiscussionAsync(Guid discussionId);
}