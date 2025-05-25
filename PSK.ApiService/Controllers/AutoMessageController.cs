using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Services.Interfaces;
using System.Security.Claims;

namespace PSK.ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class AutoMessageController : ControllerBase
    {
        private readonly IAutoMessageService _service;

        public AutoMessageController(IAutoMessageService service)
        {
            _service = service;
        }

        [HttpPost("send-random")]
        public async Task<IActionResult> SendRandomPositiveMessage()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email claim not found.");

            var result = await _service.EnqueueRandomPositiveMessageAsync(email);
            if (!result)
                return NotFound("No active positive messages found.");

            return Ok(new { message = "Positive message enqueued successfully." });
        }
    }
}
