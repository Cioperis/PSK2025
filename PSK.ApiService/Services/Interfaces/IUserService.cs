using System.Threading.Tasks;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDTO dto);
        Task UserSubscribed(Guid id);
        Task<User?> AuthenticateAsync(string email, string password);
    }
}