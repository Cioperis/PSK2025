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
    private readonly IUserRepository _userRepository;

    public CommentService(
        ICommentRepository commentRepository,
        IDiscussionRepository discussionRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _discussionRepository = discussionRepository;
        _userRepository = userRepository;

    }

    public async Task<CommentDTO> CreateCommentAsync(CreateCommentSchema comment, Guid userId)
    {
        Discussion? discussion = await _discussionRepository.GetByIdAsync(comment.DiscussionId);
        if (discussion == null)
            throw new Exception($"Comment's parent Discussion {comment.DiscussionId} not found");

        var user = _userRepository.GetById(userId);

        Comment newComment = new Comment
        {
            Content = comment.Content,
            DiscussionId = comment.DiscussionId,
            Discussion = discussion,
            UserId = userId,
        };

        await _commentRepository.AddAsync(newComment);
        await _commentRepository.SaveChangesAsync();
        return new CommentDTO
        {
            Id = newComment.Id,
            Content = newComment.Content,
            DiscussionId = newComment.DiscussionId,
            UpdatedAt = newComment.UpdatedAt,
            Username = user.FirstName
        };
    }

    public async Task<CommentDTO> UpdateCommentAsync(CommentDTO comment, Guid userId)
    {
        Comment? commentToUpdate = await _commentRepository.GetByIdAsync(comment.Id);
        if (commentToUpdate == null)
            throw new Exception($"Comment {comment.Id} not found");

        if (commentToUpdate.UserId != userId)
            throw new Exception($"Unauthorized");

        var user = _userRepository.GetById(userId);

        commentToUpdate.Content = comment.Content;
        commentToUpdate.DiscussionId = comment.DiscussionId;
        commentToUpdate.Version++;

        _commentRepository.Update(commentToUpdate);
        await _commentRepository.SaveChangesAsync();

        var updatedCommentDto = new CommentDTO
        {
            Id = commentToUpdate.Id,
            Content = commentToUpdate.Content,
            DiscussionId = commentToUpdate.DiscussionId,
            UpdatedAt = commentToUpdate.UpdatedAt,
            Username = user.FirstName
        };

        return updatedCommentDto;
    }

    public async Task<CommentDTO?> GetCommentAsync(Guid commentId)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(commentId);

        if (comment == null)
            return null;

        return new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            DiscussionId = comment.DiscussionId,
            UpdatedAt = comment.UpdatedAt,
            Username = comment.User.FirstName
        };
    }

    public async Task<IEnumerable<CommentDTO>> GetAllCommentsAsync()
    {
        IEnumerable<Comment> comments = await _commentRepository.GetAllAsync();

        return comments.Select(comment => new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            DiscussionId = comment.DiscussionId,
            UpdatedAt = comment.UpdatedAt,
            Username = comment.User.FirstName
        });
    }

    public async Task<IEnumerable<CommentDTO>> GetAllCommentsOfDiscussionAsync(Guid discussionId)
    {
        IEnumerable<Comment> comments = await _commentRepository.GetAllCommentsByDiscussionIdAsync(discussionId);

        return comments.Select(comment => new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            DiscussionId = comment.DiscussionId,
            UpdatedAt = comment.UpdatedAt,
            Username = comment.User.FirstName
        });
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(commentId);

        if (comment == null)
            return false;

        if (comment.UserId != userId)
            throw new Exception($"Unauthorized");

        _commentRepository.Remove(comment);
        await _commentRepository.SaveChangesAsync();

        return true;
    }
}