using System.ComponentModel.DataAnnotations.Schema;

namespace PSK.ServiceDefaults.Models;

public class Comment : BaseClass
{
    public required string Content {get; set;}
    
    public required Guid DiscussionId {get; set;}
    
    [ForeignKey("DiscussionId")]
    public Discussion Discussion {get; set;}
}