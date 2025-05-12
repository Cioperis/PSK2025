using System;

namespace PSK.ServiceDefaults.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}