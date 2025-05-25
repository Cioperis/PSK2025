using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task FlipUserSubscriptionStatusAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            user.IsSubscribed = !user.IsSubscribed;
            user.UpdatedAt = DateTime.UtcNow;

            Update(user);
            await SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task AddAsync(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}