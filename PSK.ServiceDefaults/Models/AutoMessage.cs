using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Models
{
    public class AutoMessage : BaseClass
    {
        public required string Content { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
