using Microsoft.Extensions.Hosting.Schema;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDTO> CreateCommentAsync(CreateCommentSchema comment, Guid userId);
    Task<CommentDTO> UpdateCommentAsync(CommentDTO comment, Guid userId);
    Task<CommentDTO?> GetCommentAsync(Guid commentId);
    Task<IEnumerable<CommentDTO>> GetAllCommentsAsync();
    Task<IEnumerable<CommentDTO>> GetAllCommentsOfDiscussionAsync(Guid discussionId);
    Task<bool> DeleteCommentAsync(Guid commentId, Guid userId);
}