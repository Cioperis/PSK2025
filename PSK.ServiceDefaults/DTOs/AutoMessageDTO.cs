using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.DTOs
{
    public class AutoMessageDTO
    {
        public required string Email { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
