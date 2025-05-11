using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;
using PSK.ServiceDefaults.Schema;

namespace PSK.ApiService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateUserSchema> CreateUserAsync(UserDTO dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                IsActive = dto.IsActive,
                LastLogin = DateTime.UtcNow
            };

            await _repository.AddAsync(user);

            var result = new CreateUserSchema
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            return result;
        }
    }
}
