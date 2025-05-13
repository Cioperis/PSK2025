using System.ComponentModel.DataAnnotations;

namespace PSK.ServiceDefaults.DTOs;

public class DiscussionDTO
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    public DateTime UpdatedAt { get; set; }
}