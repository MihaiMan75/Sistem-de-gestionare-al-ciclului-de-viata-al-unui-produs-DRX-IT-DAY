using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Interfaces
{
    public interface IAuthService
    {
       // User? CurrentUser { get; }
        bool Login(string username, string password);
        void Logout();
    }
}
