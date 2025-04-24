using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;

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
                return BadRequest(ModelState);

            await _userService.CreateUserAsync(dto);
            return Ok(new { message = "User created successfully" });
        }
    }
}
