using PSK.ServiceDefaults.Models;

public class UserMessage : BaseClass
{
    public Guid UserId { get; set; }
    public required User User { get; set; }

    public required string Content { get; set; }
    public DateTime SendAt { get; set; } // when to send it
    public bool IsRecurring { get; set; } = false;
    public bool IsEnabled { get; set; } = true;
}