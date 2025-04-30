using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IDiscussionRepository _discussionRepository;

    public CommentService(ICommentRepository commentRepository, IDiscussionRepository discussionRepository)
    {
        _commentRepository = commentRepository;
        _discussionRepository = discussionRepository;
    }
    
    public async Task<CommentDTO> CreateCommentAsync(CommentDTO comment)
    {
        Discussion? discussion = await _discussionRepository.GetByIdAsync(comment.DiscussionId);
        if (discussion == null)
            throw new Exception($"Comment's parent Discussion {comment.DiscussionId} not found");
        
        Comment newComment = new Comment
        {
            Content = comment.Content,
            DiscussionId = comment.DiscussionId,
            Discussion = discussion
        };
        
        await _commentRepository.AddAsync(newComment);
        await _commentRepository.SaveChangesAsync();
        return new CommentDTO
        {
            Id = newComment.Id,
            Content = newComment.Content,
            DiscussionId = newComment.DiscussionId
        };
    }

    public async Task<CommentDTO?> GetCommentAsync(Guid commentId)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(commentId);
        
        if  (comment == null)
            return null;

        return new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            DiscussionId = comment.DiscussionId
        };
    }

    public async Task<IEnumerable<CommentDTO>> GetAllCommentsAsync()
    {
        IEnumerable<Comment> comments = await _commentRepository.GetAllAsync();
        
        return comments.Select(comment => new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            DiscussionId = comment.DiscussionId
        });
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(commentId);
        
        if (comment == null)
            throw new Exception($"Comment {commentId} not found");
        
        _commentRepository.Remove(comment);
        await _commentRepository.SaveChangesAsync();
    }
}