using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService   _userService;
        private readonly IRabbitMQueue  _rabbitMQ;
        private readonly ITokenService  _tokenService;
        private readonly JwtSettings    _jwtSettings;

        public UserController(
            IUserService userService,
            IRabbitMQueue rabbitMQueue,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOpts)
        {
            _userService  = userService;
            _rabbitMQ     = rabbitMQueue;
            _tokenService = tokenService;
            _jwtSettings  = jwtOpts.Value;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.CreateUserAsync(dto);

            _rabbitMQ.PublishMessage(
                queue: "user.created",
                message: System.Text.Json.JsonSerializer.Serialize(new
                {
                    email     = dto.Email,
                    name      = $"{dto.FirstName} {dto.LastName}",
                    timestamp = DateTime.UtcNow
                })
            );

            return Ok(new { message = "User created successfully" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token   = _tokenService.CreateToken(user);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);

            return Ok(new AuthResponseDTO { Token = token, Expires = expires });
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
