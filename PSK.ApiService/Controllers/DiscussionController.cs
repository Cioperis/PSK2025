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
    
}