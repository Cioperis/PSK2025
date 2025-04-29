using PSK.ApiService.Data;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context) {}
}