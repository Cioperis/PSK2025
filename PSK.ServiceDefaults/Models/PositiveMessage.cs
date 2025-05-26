using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Models
{
    public class PositiveMessage : BaseClass
    {
        public required string Content { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
