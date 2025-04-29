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

    public async Task<IActionResult> CreateComment(CommentDTO comment)
    {
        CommentDTO newCommentDto = await _commentService.CreateCommentAsync(comment);
        return Ok(newCommentDto);
    }
}