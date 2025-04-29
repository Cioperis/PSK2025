using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces;

public interface IDiscussionRepository
{
    Task<Discussion?> GetDiscussionByIdAsync(Guid discussionId);
    Task<List<Discussion?>> GetAllDiscussionsAsync();
    Task<Discussion> AddDiscussionAsync(Discussion discussion);
    Task DeleteDiscussionAsync(Discussion discussion);
}