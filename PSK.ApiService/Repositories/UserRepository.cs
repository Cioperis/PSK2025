using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) {}

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
