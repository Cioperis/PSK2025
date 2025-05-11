using System.ComponentModel.DataAnnotations;

namespace PSK.ServiceDefaults.DTOs
{
    public class LoginUserDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }
}