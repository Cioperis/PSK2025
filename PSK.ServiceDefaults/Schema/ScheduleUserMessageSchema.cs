using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Schema
{
    public class ScheduleUserMessageResult
    {
        public bool Success { get; set; }
        public Guid MessageId { get; set; }
        public required string ErrorMessage { get; set; }
    }
}
