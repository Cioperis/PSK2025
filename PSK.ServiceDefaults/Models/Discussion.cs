namespace PSK.ServiceDefaults.Models;

public class Discussion : BaseClass
{
    public required string Name {get; set;}
    public List<Comment>? Comments {get; set;}
}