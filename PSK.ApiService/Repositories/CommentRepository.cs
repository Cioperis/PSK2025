using Microsoft.EntityFrameworkCore;
using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context) {}

    public async Task<IEnumerable<Comment>> GetAllCommentsByDiscussionIdAsync(Guid discussionId)
    {
        return await _context.Comments.Where(comment => comment.DiscussionId == discussionId).ToListAsync();
    }
}