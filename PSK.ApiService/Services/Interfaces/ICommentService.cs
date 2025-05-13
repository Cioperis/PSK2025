using Microsoft.Extensions.Hosting.Schema;
using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDTO> CreateCommentAsync(CreateCommentSchema comment);
    Task<CommentDTO> UpdateCommentAsync(CommentDTO comment);
    Task<CommentDTO?> GetCommentAsync(Guid commentId);
    Task<IEnumerable<CommentDTO>> GetAllCommentsAsync();
    Task<IEnumerable<CommentDTO>> GetAllCommentsOfDiscussionAsync(Guid discussionId);
    Task<bool> DeleteCommentAsync(Guid commentId);
}