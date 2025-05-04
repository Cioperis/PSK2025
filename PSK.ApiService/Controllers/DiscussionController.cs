using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Schema;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;
using Serilog;

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
    [ProducesResponseType(typeof(DiscussionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDiscussion([FromBody] CreateDiscussionSchema discussion)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Invalid model state for CreateDiscussion request: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            DiscussionDTO newDiscussionDto = await _discussionService.CreateDiscussionAsync(discussion);
            Log.Information("Discussion created successfully. Discussion data: {@Discussion}", newDiscussionDto);
            return Ok(newDiscussionDto);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create discussion. Request: {@DiscussionRequest}", discussion);
            return StatusCode(500, "An error occurred while creating the discussion");
        }
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(DiscussionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDiscussion([FromBody] DiscussionDTO discussion)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Invalid model state for UpdateDiscussion request: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            DiscussionDTO updatedDiscussion = await _discussionService.UpdateDiscussionAsync(discussion);
            Log.Information("Discussion updated successfully. Discussion ID: {DiscussionId}, Data: {@Discussion}", discussion.Id, updatedDiscussion);
            return Ok(updatedDiscussion);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update discussion. Discussion ID: {DiscussionId}", discussion.Id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DiscussionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDiscussionById([FromRoute] Guid id)
    {
        try
        {
            DiscussionDTO? discussionDto = await _discussionService.GetDiscussionAsync(id);
            
            if (discussionDto == null)
            {
                Log.Warning("Discussion not found. Discussion ID: {DiscussionId}", id);
                return NotFound();
            }

            Log.Information("Retrieved discussion successfully. Discussion ID: {DiscussionId}", id);
            return Ok(discussionDto);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving discussion. Discussion ID: {DiscussionId}", id);
            return StatusCode(500, "An error occurred while fetching the discussion");
        }
    }
    
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<DiscussionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllDiscussions()
    {
        try
        {
            var discussions = await _discussionService.GetAllDiscussionsAsync();
            Log.Information("Retrieved all discussions. Count: {DiscussionCount}", discussions.Count());
            return Ok(discussions);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving all discussions");
            return StatusCode(500, "An error occurred while fetching discussions");
        }
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDiscussion([FromRoute] Guid id)
    {
        try
        {
            bool isDeleted = await _discussionService.DeleteDiscussionAsync(id);
            
            if (!isDeleted)
            {
                Log.Warning("Delete failed - discussion not found. Discussion ID: {DiscussionId}", id);
                return NotFound($"Discussion with id {id} not found");
            }

            Log.Information("Discussion deleted successfully. Discussion ID: {DiscussionId}", id);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting discussion. Discussion ID: {DiscussionId}", id);
            return StatusCode(500, "An error occurred while deleting the discussion");
        }
    }
}