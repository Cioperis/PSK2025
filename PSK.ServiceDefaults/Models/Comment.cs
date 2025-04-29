using System.ComponentModel.DataAnnotations.Schema;

namespace PSK.ServiceDefaults.Models;

public class Comment
{
    public required string Content {get; set;}
    
    [ForeignKey("DiscussionId")]
    public required Discussion Discussion {get; set;}
}