using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK.ServiceDefaults.Schema
{
    public class CreateUserSchema
    {
        public Guid Id { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String Email { get; set; }
    }
}
