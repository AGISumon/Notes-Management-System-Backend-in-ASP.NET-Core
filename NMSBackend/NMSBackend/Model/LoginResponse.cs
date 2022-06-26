using System;
using System.Collections.Generic;

namespace NMSBackend.Model
{
    public class LoginResponse
    {
        public User User { get; set; }
        public Token Token { get; set; }
    }
}
