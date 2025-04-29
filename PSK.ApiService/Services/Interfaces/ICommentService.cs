using PSK.ServiceDefaults.DTOs;

namespace PSK.ApiService.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDTO> CreateCommentAsync(CommentDTO comment);
    Task<CommentDTO?> GetCommentAsync(Guid commentId);
    Task<IEnumerable<CommentDTO>> GetAllCommentsAsync();
    Task DeleteCommentAsync(Guid commentId);
}