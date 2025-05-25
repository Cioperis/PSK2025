using System.Threading.Tasks;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;
using PSK.ServiceDefaults.Schema;

namespace PSK.ApiService.Services.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserSchema> CreateUserAsync(UserDTO dto);
        Task<User?> AuthenticateAsync(string email, string password);
    }
}