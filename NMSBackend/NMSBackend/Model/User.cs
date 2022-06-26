using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMSBackend.Model
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}
