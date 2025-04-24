using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
    }
}
