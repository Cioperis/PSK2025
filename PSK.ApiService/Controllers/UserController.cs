using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Caching.Interfaces;
using Microsoft.Extensions.Options;
using PSK.ApiService.Authentication;
using PSK.ApiService.Messaging.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using Serilog;

namespace PSK.ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRabbitMQueue _rabbitMQ;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ICacheService _cache;

        public UserController(
            IUserService userService,
            IRabbitMQueue rabbitMQueue,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOpts,
            ICacheService cache)
        {
            _userService = userService;
            _rabbitMQ = rabbitMQueue;
            _tokenService = tokenService;
            _jwtSettings = jwtOpts.Value;
            _cache = cache;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdUser = await _userService.CreateUserAsync(dto);

                await _cache.SetAsync($"user:id:{createdUser.Id}", createdUser, TimeSpan.FromHours(1));

                _rabbitMQ.PublishMessage(
                    queue: "user.created",
                    message: System.Text.Json.JsonSerializer.Serialize(new
                    {
                        email = dto.Email,
                        name = $"{dto.FirstName} {dto.LastName}",
                        role = dto.Role.ToString(),
                        timestamp = DateTime.UtcNow
                    })
                );

                Log.Information("User created successfully: {Email}, role {Role}", dto.Email, dto.Role);
                return Ok(new { message = "User created successfully" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating user: {Email}", dto.Email);
                return StatusCode(500, new { message = "An error occurred while creating the user" });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);
                if (user == null)
                {
                    Log.Warning("Invalid login attempt: {Email}", dto.Email);
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var token = _tokenService.CreateToken(user);
                var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);

                Log.Information("User logged in successfully: {Email}", dto.Email);
                return Ok(new AuthResponseDTO
                {
                    Token = token,
                    Expires = expires
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during login: {Email}", dto.Email);
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [Authorize]
        [HttpGet("Me")]
        public IActionResult Me()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { Email = email });
        }
    }
}