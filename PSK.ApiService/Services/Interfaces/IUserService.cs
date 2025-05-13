using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDTO dto);
        Task UserSubscribed(Guid id);
    }
}
