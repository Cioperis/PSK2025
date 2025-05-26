using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;

namespace PSK.ApiService.Repositories
{
    public class UserMessageRepository : BaseRepository<UserMessage>, IUserMessageRepository
    {
        public UserMessageRepository(AppDbContext context) : base(context) { }

        public async Task<UserMessage?> GetWithUserAsync(Guid id)
        {
            return await _context.UserMessage
                .Include(um => um.User)
                .FirstOrDefaultAsync(um => um.Id == id);
        }

        public async Task<UserMessage> AddAsync(UserMessage entity)
        {
            _context.UserMessage.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(UserMessage entity)
        {
            _context.UserMessage.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

}
