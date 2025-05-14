using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<User> _hasher;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<CreateUserSchema> CreateUserAsync(UserDTO dto)
        {
            var hashedPassword = _hasher.HashPassword(null!, dto.Password);
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword,
                IsActive = dto.IsActive,
                LastLogin = DateTime.UtcNow,
                Role = dto.Role
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