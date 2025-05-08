using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories
{
    public class AutoMessageRepository : BaseRepository<AutoMessage>, IAutoMessageRepository
    {
        public AutoMessageRepository(AppDbContext context) : base(context) { }

        public async Task<AutoMessage?> GetRandomActiveMessageAsync()
        {
            var activeMessages = await _context.AutoMessages
                .Where(m => m.IsActive)
                .ToListAsync();

            if (!activeMessages.Any())
                return null;

            var random = new Random();
            return activeMessages[random.Next(activeMessages.Count)];
        }

        public async Task<IEnumerable<AutoMessage>> SearchByKeywordAsync(string keyword)
        {
            return await _context.AutoMessages
                .Where(m => m.Content.Contains(keyword))
                .ToListAsync();
        }

        public async Task<int> CountActiveAsync()
        {
            return await _context.AutoMessages.CountAsync(m => m.IsActive);
        }

        public async Task<int> CountInactiveAsync()
        {
            return await _context.AutoMessages.CountAsync(m => !m.IsActive);
        }

        public async Task<IEnumerable<AutoMessage>> GetRecentAsync(int count = 10)
        {
            return await _context.AutoMessages
                .OrderByDescending(m => m.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
