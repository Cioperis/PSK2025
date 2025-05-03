using Microsoft.AspNetCore.Mvc;
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

        public UserController(IUserService userService, IConnection rabbitConnection)
        {
            _userService = userService;

            var channel = rabbitConnection.CreateModel();


            channel.QueueDeclare(queue: "catalogEvents",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes("Getting all items in the catalog.");

            channel.BasicPublish(exchange: string.Empty,
                routingKey: "catalogEvents",
                basicProperties: null,
                body: body);
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
