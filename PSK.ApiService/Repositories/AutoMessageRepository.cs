using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Repositories;
using PSK.ServiceDefaults.Models;
using Microsoft.EntityFrameworkCore;

public class AutoMessageRepository : BaseRepository<PositiveMessage>, IAutoMessageRepository
{
    public AutoMessageRepository(AppDbContext context) : base(context) { }

    public async Task<PositiveMessage?> GetRandomActiveMessageAsync()
    {
        return await _context.PositiveMessage
            .Where(m => m.IsEnabled)
            .OrderBy(m => Guid.NewGuid())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PositiveMessage>> SearchByKeywordAsync(string keyword)
    {
        return await _context.PositiveMessage
            .Where(m => m.Content.Contains(keyword))
            .ToListAsync();
    }

    public async Task<int> CountActiveAsync()
    {
        return await _context.PositiveMessage.CountAsync(m => m.IsEnabled);
    }

    public async Task<int> CountInactiveAsync()
    {
        return await _context.PositiveMessage.CountAsync(m => !m.IsEnabled);
    }

    public async Task<IEnumerable<PositiveMessage>> GetRecentAsync(int count = 10)
    {
        return await _context.PositiveMessage
            .OrderByDescending(m => m.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
