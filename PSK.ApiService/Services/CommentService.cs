using Microsoft.Extensions.Hosting.Schema;
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
    
    public async Task<CommentDTO> CreateCommentAsync(CreateCommentSchema comment)
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

    public async Task<CommentDTO> UpdateCommentAsync(CommentDTO comment)
    {
        Comment? commentToUpdate = await _commentRepository.GetByIdAsync(comment.Id);
        if (commentToUpdate == null)
            throw new Exception($"Comment {comment.Id} not found");

        commentToUpdate.Content = comment.Content;
        commentToUpdate.DiscussionId = comment.DiscussionId;
        
        _commentRepository.Update(commentToUpdate);
        await _commentRepository.SaveChangesAsync();
        
        var updatedCommentDto = new CommentDTO 
        {
            Id = commentToUpdate.Id,
            Content = commentToUpdate.Content,
            DiscussionId = commentToUpdate.DiscussionId,
        };
        
        return updatedCommentDto;
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

    public async Task<bool> DeleteCommentAsync(Guid commentId)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(commentId);

        if (comment == null)
            return false;
        
        _commentRepository.Remove(comment);
        await _commentRepository.SaveChangesAsync();
        
        return true;
    }
}