using System.ComponentModel.DataAnnotations;

namespace Microsoft.Extensions.Hosting.Schema;

public class CreateCommentSchema
{
    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
    [Required(ErrorMessage = "DiscussionId is required")]
    public Guid DiscussionId { get; set; }
}