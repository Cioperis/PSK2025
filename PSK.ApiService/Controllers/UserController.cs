using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Caching.Interfaces;
using PSK.ApiService.Messaging.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PSK.ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRabbitMQueue _rabbitMQ;
        private readonly ICacheService _cache;

        public UserController(IUserService userService, IRabbitMQueue rabbitMQueue, ICacheService cache)
        {
            _userService = userService;
            _rabbitMQ = rabbitMQueue;
            _cache = cache;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for CreateUser request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

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
                        timestamp = DateTime.UtcNow
                    })
                );

                Log.Information("User created successfully: {@UserDTO}", dto);
                return Ok(new { message = "User created successfully" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating a user: {@UserDTO}", dto);
                return StatusCode(500, new { message = "An error occurred while creating the user" });
            }
        }
    }
}
