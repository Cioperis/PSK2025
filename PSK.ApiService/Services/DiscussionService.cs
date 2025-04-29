using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services;

public class DiscussionService : IDiscussionService
{
    private readonly IDiscussionRepository _repository;

    public DiscussionService(IDiscussionRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<DiscussionDTO> CreateDiscussionAsync(DiscussionDTO discussion)
    {
        Discussion newDiscussion = new Discussion
        {
            Id = discussion.Id,
            Name = discussion.Name
        };
        
        await _repository.AddAsync(newDiscussion);
        return new DiscussionDTO
        {
            Id = newDiscussion.Id,
            Name = newDiscussion.Name
        };
    }

    public async Task<IEnumerable<DiscussionDTO>> GetAllDiscussionsAsync()
    {
        IEnumerable<Discussion> discussions = await _repository.GetAllAsync();

        return discussions.Select(discussion => new DiscussionDTO
        {
            Id = discussion.Id,
            Name = discussion.Name
        });
    }

    public async Task<DiscussionDTO?> GetDiscussionAsync(Guid discussionId)
    {
        Discussion? discussion = await _repository.GetByIdAsync(discussionId);

        if (discussion == null)
            return null;

        return new DiscussionDTO
        {
            Id = discussion.Id,
            Name = discussion.Name
        };
    }

    public async Task DeleteDiscussionAsync(Guid discussionId)
    {
        Discussion? discussion = await _repository.GetByIdAsync(discussionId);
        
        if  (discussion == null)
            throw new Exception($"Discussion {discussionId} not found");
        
        _repository.Remove(discussion);
    }
}