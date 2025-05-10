using Microsoft.AspNetCore.Mvc;
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

        public UserController(IUserService userService, IRabbitMQueue rabbitMQueue)
        {
            _userService = userService;
            _rabbitMQ = rabbitMQueue;
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
                await _userService.CreateUserAsync(dto);

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
