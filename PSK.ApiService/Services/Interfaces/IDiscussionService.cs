using Microsoft.Extensions.Hosting.Schema;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services.Interfaces;

public interface IDiscussionService
{
    Task<DiscussionDTO> CreateDiscussionAsync(CreateDiscussionSchema discussion, Guid userId);
    Task<DiscussionDTO> UpdateDiscussionAsync(DiscussionDTO discussion, Guid userId);
    Task<IEnumerable<DiscussionDTO>> GetAllDiscussionsAsync();
    Task<DiscussionDTO?> GetDiscussionAsync(Guid discussionId);
    Task<bool> DeleteDiscussionAsync(Guid discussionId, Guid userId);
}