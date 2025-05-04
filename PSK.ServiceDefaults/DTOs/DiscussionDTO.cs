using System.ComponentModel.DataAnnotations;

namespace PSK.ServiceDefaults.DTOs;

public class DiscussionDTO
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
}