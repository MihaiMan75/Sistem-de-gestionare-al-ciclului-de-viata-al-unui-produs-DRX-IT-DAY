using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.Interfaces
{
    public interface IAuthService
    {
        UserDto CurrentUser { get; }
       // event EventHandler<UserDto> CurrentUserChanged;
        Task<bool> Login(string username, string password);
        bool VerifyPassword(string password, string hashedPassword);
        void Logout();
    }
}
