namespace PSK.ServiceDefaults.DTOs;

public class UserMessageDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Content { get; set; }
    public DateTime SendAt { get; set; } // when to send it
    public bool IsRecurring { get; set; } = false;
    public bool IsEnabled { get; set; } = true;
}