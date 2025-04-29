using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.DTOs;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;

    CommentService(ICommentRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<CommentDTO> CreateCommentAsync(CommentDTO comment)
    {
        Comment newComment = new Comment
        {
            Content = comment.Content,
            DiscussionId = comment.DiscussionId
        };
        
        await _repository.AddAsync(newComment);
        return new CommentDTO
        {
            Id = newComment.Id,
            Content = newComment.Content,
            DiscussionId = newComment.DiscussionId
        };
    }

    public async Task<CommentDTO?> GetCommentAsync(Guid commentId)
    {
        Comment? comment = await _repository.GetByIdAsync(commentId);
        
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
        IEnumerable<Comment> comments = await _repository.GetAllAsync();
        
        return comments.Select(comment => new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            DiscussionId = comment.DiscussionId
        });
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        Comment? comment = await _repository.GetByIdAsync(commentId);
        
        if (comment == null)
            throw new Exception($"Comment {commentId} not found");
        
        _repository.Remove(comment);
    }
}