using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Services.Interfaces;

namespace PSK.ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutoMessageController : ControllerBase
    {
        private readonly IAutoMessageService _service;

        public AutoMessageController(IAutoMessageService service)
        {
            _service = service;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var message = await _service.GetRandomMessageAsync();
            if (message == null)
                return NotFound("No active messages found.");
            return Ok(message);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var results = await _service.SearchMessagesAsync(keyword);
            return Ok(results);
        }
    }
}
