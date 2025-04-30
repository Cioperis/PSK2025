using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscussionController : ControllerBase
{
    private readonly IDiscussionService  _discussionService;

    public DiscussionController(IDiscussionService discussionService)
    {
        _discussionService = discussionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscussion([FromBody] DiscussionDTO discussion)
    {
        DiscussionDTO newDiscussionDto = await _discussionService.CreateDiscussionAsync(discussion);
        return Ok(newDiscussionDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscussionById([FromRoute] Guid id)
    {
        DiscussionDTO? discussionDto = await _discussionService.GetDiscussionAsync(id);
        return Ok(discussionDto);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllDiscussions()
    {
        IEnumerable<DiscussionDTO> discussionDto = await _discussionService.GetAllDiscussionsAsync();
        return Ok(discussionDto);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDiscussion([FromQuery] Guid id)
    {
        try
        {
            await _discussionService.DeleteDiscussionAsync(id);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
        
        return Ok();
    }
}