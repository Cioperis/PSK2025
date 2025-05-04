using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Schema;
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
    public async Task<IActionResult> CreateDiscussion([FromBody] CreateDiscussionSchema discussion)
    {
        DiscussionDTO newDiscussionDto = await _discussionService.CreateDiscussionAsync(discussion);
        return Ok(newDiscussionDto);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<IActionResult> UpdateDiscussion([FromBody] DiscussionDTO discussion)
    {
        try
        {
            DiscussionDTO updatedDiscussion = await _discussionService.UpdateDiscussionAsync(discussion);
            return Ok(updatedDiscussion);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscussion([FromRoute] Guid id)
    {
        if (!await _discussionService.DeleteDiscussionAsync(id))
            return NotFound($"Discussion with id {id} not found");
            
        return Ok();
    }
}