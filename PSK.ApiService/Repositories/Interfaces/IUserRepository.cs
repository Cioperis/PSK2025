using System.Threading.Tasks;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task FlipUserSubscriptionStatusAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
    }
}