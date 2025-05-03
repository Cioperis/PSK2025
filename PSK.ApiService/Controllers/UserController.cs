using Microsoft.AspNetCore.Mvc;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
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
                Log.Fatal(ex, "An error occurred while creating a user: {@UserDTO}", dto);
                return StatusCode(500, new { message = "An error occurred while creating the user" });
            }
        }
    }
}
