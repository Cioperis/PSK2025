using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSK.ServiceDefaults.Models;

public class Discussion : BaseClass
{
    public required string Name { get; set; }

    public ICollection<Comment>? Comments { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    [Required]
    public Guid UserId { get; set; }
}