using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories;

public class CommentRepository : ICommentRepository
{
    public Task<Comment> GetCommentByIdAsync(string commentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Comment>> GetAllCommentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Comment> AddCommentAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCommentAsync(Comment comment)
    {
        throw new NotImplementedException();
    }
}