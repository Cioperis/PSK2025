using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task FlipUserSubscriptionStatusAsync(Guid userId);
    }
}
