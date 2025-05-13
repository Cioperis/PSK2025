using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateUserAsync(UserDTO dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                IsActive = dto.IsActive,
                IsSubscribed = dto.IsSubscribed,
                LastLogin = DateTime.UtcNow
            };

            await _repository.AddAsync(user);
        }

        public async Task UserSubscribed(Guid id)
        {
            await _repository.FlipUserSubscriptionStatusAsync(id);
        }
    }
}
