using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> GetAllCommentsByDiscussionIdAsync(Guid discussionId);
}