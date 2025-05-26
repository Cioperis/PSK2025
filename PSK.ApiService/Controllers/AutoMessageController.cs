using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Schema;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("send-random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendRandomPositiveMessage()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
    
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email not found in claims");
            
            var result = await _service.EnqueueRandomPositiveMessageAsync(userEmail);
            if (!result)
                return NotFound("No active positive messages found.");

            return Ok(new { message = "Positive message enqueued successfully." });
        }

        [Authorize]
        [HttpPost("schedule-messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ScheduleMessages([FromBody] ScheduleMessageRequest request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (request.Days < 1 || request.Days > 30)
                return BadRequest("Invalid request.");
    
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email not found in claims");

            for (int i = 0; i < request.Days; i++)
            {
                var scheduledDate = DateTime.UtcNow.Date.AddDays(i).AddHours(9);
                Hangfire.BackgroundJob.Schedule<IAutoMessageService>(
                    service => service.EnqueueRandomPositiveMessageAsync(userEmail),
                    scheduledDate
                );
            }
            return Ok(new { message = $"Message scheduled for the next {request.Days} days at 9am." });
        }

        [Authorize]
        [HttpPost("schedule-custom-message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ScheduleCustomMessage([FromBody] UserMessageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.SendAt <= DateTime.UtcNow)
                return BadRequest("SendAt must be in the future.");

            // retrieve using jwt (authenticated use only)
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Invalid or missing user identity.");

            var result = await _service.ScheduleUserCustomMessageAsync(userId, request);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            var messageDto = _service.GetCustomMessage(result.MessageId);

            return Ok(messageDto);
        }
        
        [Authorize]
        [HttpGet("GetAllMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllMessages()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Invalid or missing user identity.");

            var customMessages = await _service.GetUsersCustomMessages(userId);
            
            return Ok(customMessages);
        }
        
        [Authorize]
        [HttpDelete("{messageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteMessage([FromRoute] Guid messageId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Invalid or missing user identity.");
            
            bool isDeleted = await _service.DeleteUserMessage(messageId, userId);

            if (!isDeleted)
                return Unauthorized("Message doesn't exist or permission denied");
            
            return Ok();
        }

    }
}
