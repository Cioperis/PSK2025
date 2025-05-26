using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Schema
{
    public class UserMessageRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "Content must not be empty.")]
        public required string Content { get; set; }

        [Required]
        public DateTime SendAt { get; set; }

        public bool IsRecurring { get; set; }
    }

}
