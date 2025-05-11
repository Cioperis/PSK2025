using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Models
{
    public class User : BaseClass
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
        public required string Role { get; set; } = "User";
    }
}