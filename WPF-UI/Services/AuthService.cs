using BusinessLogic;
using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.Interfaces;
using DataAccess.Models;
using ProductManagementBusinessLogic.AuthUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.Interfaces;
using WPF_UI.Messages;

namespace WPF_UI.Services
{
    public partial class AuthService : ObservableObject, IAuthService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IServiceFactory _serviceFactory;
        private readonly IUserService _userService;

        [ObservableProperty]
        private UserDto _currentUser;

        public AuthService(IServiceFactory serviceFactory)
        {
            _passwordHasher = new PasswordHasher();
            _serviceFactory = serviceFactory;
            _userService = _serviceFactory.GetUserService();
        }

        private void UpdateCurrentUser(UserDto user)
        {
            WeakReferenceMessenger.Default.Send(new UserChangedMessage(user));
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await _userService.GetUserByUserNameAsync(username);
            if (user != null)
            {
                if (VerifyPassword(password, user.PasswordHashed))
                {
                    CurrentUser = user;
                    return true;
                }
            }
            return false;
        }

        partial void OnCurrentUserChanged(UserDto value)
        {
            UpdateCurrentUser(value);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
           return _passwordHasher.VerifyPassword(password, hashedPassword);
        }

        [RelayCommand]
        public void Logout()
        {
            CurrentUser = null;
        }


    }
}
