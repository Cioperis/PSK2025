using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly PasswordHasher<User> _hasher;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
            _hasher = new PasswordHasher<User>();
        }

        public async Task CreateUserAsync(UserDTO dto)
        {
            var hashedPassword = _hasher.HashPassword(null!, dto.Password);
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword,
                IsActive = dto.IsActive,
                IsSubscribed = dto.IsSubscribed,
                LastLogin = DateTime.UtcNow,
                Role = dto.Role
            };

            await _repository.AddAsync(user);
        }

        public async Task UserSubscribed(Guid id)
        {
            await _repository.FlipUserSubscriptionStatusAsync(id);
        }
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
                return null;

            var result = _hasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Failed ? null : user;
        }
    }
}