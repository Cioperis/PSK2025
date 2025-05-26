using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSK.ServiceDefaults.Models;

public class Comment : BaseClass
{
    public required string Content { get; set; }

    public required Guid DiscussionId { get; set; }

    [ForeignKey("DiscussionId")]
    public Discussion Discussion { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    [Required]
    public Guid UserId { get; set; }
}