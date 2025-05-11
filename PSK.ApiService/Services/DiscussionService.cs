using Microsoft.Extensions.Hosting.Schema;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services;

public class DiscussionService : IDiscussionService
{
    private readonly IDiscussionRepository _discussionRepository;

    public DiscussionService(IDiscussionRepository discussionRepository)
    {
        _discussionRepository = discussionRepository;
    }

    public async Task<DiscussionDTO> CreateDiscussionAsync(CreateDiscussionSchema discussion)
    {
        Discussion newDiscussion = new Discussion
        {
            Name = discussion.Name
        };

        await _discussionRepository.AddAsync(newDiscussion);
        await _discussionRepository.SaveChangesAsync();
        return new DiscussionDTO
        {
            Id = newDiscussion.Id,
            Name = newDiscussion.Name,
            UpdatedAt = newDiscussion.UpdatedAt
        };
    }

    public async Task<DiscussionDTO> UpdateDiscussionAsync(DiscussionDTO discussion)
    {
        Discussion? discussionToUpdate = await _discussionRepository.GetByIdAsync(discussion.Id);
        if (discussionToUpdate == null)
            throw new Exception($"Discussion {discussion.Id} not found");

        discussionToUpdate.Name = discussion.Name;

        _discussionRepository.Update(discussionToUpdate);
        await _discussionRepository.SaveChangesAsync();

        var updatedDiscussionDto = new DiscussionDTO
        {
            Id = discussion.Id,
            Name = discussion.Name,
            UpdatedAt = discussion.UpdatedAt
        };

        return updatedDiscussionDto;
    }

    public async Task<IEnumerable<DiscussionDTO>> GetAllDiscussionsAsync()
    {
        IEnumerable<Discussion> discussions = await _discussionRepository.GetAllAsync();

        return discussions.Select(discussion => new DiscussionDTO
        {
            Id = discussion.Id,
            Name = discussion.Name,
            UpdatedAt = discussion.UpdatedAt
        });
    }

    public async Task<DiscussionDTO?> GetDiscussionAsync(Guid discussionId)
    {
        Discussion? discussion = await _discussionRepository.GetByIdAsync(discussionId);

        if (discussion == null)
            return null;

        return new DiscussionDTO
        {
            Id = discussion.Id,
            Name = discussion.Name,
            UpdatedAt = discussion.UpdatedAt
        };
    }

    public async Task<bool> DeleteDiscussionAsync(Guid discussionId)
    {
        Discussion? discussion = await _discussionRepository.GetByIdAsync(discussionId);

        if (discussion == null)
            return false;

        _discussionRepository.Remove(discussion);
        await _discussionRepository.SaveChangesAsync();

        return true;
    }
}