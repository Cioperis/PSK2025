using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Schema;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using Serilog;

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
    [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentSchema comment)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Invalid model state for CreateComment request: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            CommentDTO newCommentDto = await _commentService.CreateCommentAsync(comment);
            Log.Information("Comment created successfully. Comment: {@CreateCommentSchema}", comment);
            return Ok(newCommentDto);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create comment. Request: {@CommentRequest}", comment);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateComment([FromBody] CommentDTO comment)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("Invalid model state for UpdateComment request: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            CommentDTO updatedComment = await _commentService.UpdateCommentAsync(comment);
            Log.Information("Comment updated successfully. Comment ID: {CommentId}, Data: {@CommentDTO}", comment.Id, comment);
            return Ok(updatedComment);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update comment. Comment ID: {CommentId}, Data: {@CommentDTO}", comment.Id, comment);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
    {
        try
        {
            CommentDTO? commentDto = await _commentService.GetCommentAsync(id);

            if (commentDto == null)
            {
                Log.Warning("Comment not found. Comment ID: {CommentId}", id);
                return NotFound();
            }

            Log.Information("Retrieved comment successfully. Comment ID: {CommentId}", id);
            return Ok(commentDto);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving comment. Comment ID: {CommentId}", id);
            return StatusCode(500, "An error occurred while fetching the comment");
        }
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<CommentDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllComments()
    {
        try
        {
            var commentDtos = await _commentService.GetAllCommentsAsync();
            Log.Information("Retrieved all comments. Count: {CommentCount}", commentDtos.Count());
            return Ok(commentDtos);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving all comments");
            return StatusCode(500, "An error occurred while fetching comments");
        }
    }

    [HttpGet("ofDiscussion/{discussionId}")]
    [ProducesResponseType(typeof(IEnumerable<CommentDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCommentsOfDiscussion(Guid discussionId)
    {
        try
        {
            var commentDtos = await _commentService.GetAllCommentsOfDiscussionAsync(discussionId);
            Log.Information("Retrieved all comments of discussion {discussionId}. " +
                            "Count: {CommentCount}", discussionId, commentDtos.Count());
            return Ok(commentDtos);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving comments");
            return StatusCode(500, "An error occurred while fetching comments");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
    {
        try
        {
            bool isDeleted = await _commentService.DeleteCommentAsync(id);

            if (!isDeleted)
            {
                Log.Warning("Delete failed - comment not found. Comment ID: {CommentId}", id);
                return NotFound($"Comment with id {id} not found");
            }

            Log.Information("Comment deleted successfully. Comment ID: {CommentId}", id);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting comment. Comment ID: {CommentId}", id);
            return StatusCode(500, "An error occurred while deleting the comment");
        }
    }
}