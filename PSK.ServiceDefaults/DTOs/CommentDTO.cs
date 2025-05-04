using System.ComponentModel.DataAnnotations;

namespace PSK.ServiceDefaults.DTOs;

public class CommentDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
    [Required(ErrorMessage = "DiscussionId is required")]
    public Guid DiscussionId { get; set; }
}