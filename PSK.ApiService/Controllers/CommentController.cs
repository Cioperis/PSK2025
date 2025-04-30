using Microsoft.AspNetCore.Mvc;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(CommentDTO comment)
    {
        try
        {
            CommentDTO newCommentDto = await _commentService.CreateCommentAsync(comment);
            return Ok(newCommentDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
    {
        CommentDTO? commentDto = await _commentService.GetCommentAsync(id);
        return Ok(commentDto);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllComments()
    {
        IEnumerable<CommentDTO> commentDtos = await _commentService.GetAllCommentsAsync();
        return Ok(commentDtos);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromQuery] Guid id)
    {
        try
        {
            await _commentService.DeleteCommentAsync(id);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
        
        return Ok();
    }
}