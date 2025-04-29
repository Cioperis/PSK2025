using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<Comment> GetCommentByIdAsync(string commentId);
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment> AddCommentAsync(Comment comment);
    Task DeleteCommentAsync(Comment comment);
}