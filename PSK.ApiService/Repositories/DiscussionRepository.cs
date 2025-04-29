using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories;

public class DiscussionRepository : IDiscussionRepository
{
    
    private readonly AppDbContext _context;

    public DiscussionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Discussion?> GetDiscussionByIdAsync(Guid discussionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Discussion?>> GetAllDiscussionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Discussion> AddDiscussionAsync(Discussion discussion)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDiscussionAsync(Discussion discussion)
    {
        throw new NotImplementedException();
    }
}