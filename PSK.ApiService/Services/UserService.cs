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
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = "",
                IsActive = dto.IsActive,
                LastLogin = DateTime.UtcNow,
                Role = dto.Role
            };

            user.Password = _hasher.HashPassword(user, dto.Password);

            await _repository.AddAsync(user);
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
                return null;

            var res = _hasher.VerifyHashedPassword(user, user.Password, password);
            return res == PasswordVerificationResult.Failed ? null : user;
        }
    }
}