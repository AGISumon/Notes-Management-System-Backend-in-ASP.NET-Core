using NMSBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMSBackend.Interface
{
    public interface IUserService
    {
        LoginResponse Login(User user, out string message);
        //LoginResponse RefreshToken(Token token, out string message);
        bool Logout(string emailId);
        bool SignUp(User user);
    }
}
