using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Interfaces;

namespace UI.Services
{
    public class AuthService : IAuthService
    {
        //public User? CurrentUser { get; private set; }

        public bool Login(string username, string password)
        {
                          //User Verification
            if (username == "admin" && password == "password")
            {
               // CurrentUser = new User { Username = username };
                return true;
            }
            return false;
        }

        public void Logout()
        {
         //   CurrentUser = null;
        }
    }
}
