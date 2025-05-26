using System.ComponentModel.DataAnnotations;

namespace Microsoft.Extensions.Hosting.Schema;

public class CreateDiscussionSchema
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
}