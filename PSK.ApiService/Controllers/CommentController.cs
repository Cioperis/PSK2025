using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Schema;
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
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentSchema comment)
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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromBody] CommentDTO comment)
    {
        try
        {
            CommentDTO updatedComment = await _commentService.UpdateCommentAsync(comment);
            return Ok(updatedComment);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
    {
        if (!await _commentService.DeleteCommentAsync(id))
            return NotFound($"Comment with id {id} not found");

        return Ok();
    }
}