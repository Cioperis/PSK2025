using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Schema
{
    public class ScheduleMessageRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Days must be greater than 0.")]
        public int Days { get; set; }
    }
}
