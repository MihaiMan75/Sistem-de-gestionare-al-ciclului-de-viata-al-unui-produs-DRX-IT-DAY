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
using System.Windows;
using WPF_UI.Interfaces;
using WPF_UI.Messages;

namespace WPF_UI.Services
{
    public partial class AuthService : ObservableObject, IAuthService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IServiceFactory _serviceFactory;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        [ObservableProperty]
        private UserDto _currentUser;

        public AuthService(IServiceFactory serviceFactory)
        {
            _passwordHasher = new PasswordHasher();
            _serviceFactory = serviceFactory;
            _userService = _serviceFactory.GetUserService();
            _roleService = _serviceFactory.GetRoleService();
            CheckForAdmin();
            //check if in the db theres an conection ,if not error then check if ther's an admin user if not create one

        }

        private async Task CheckForAdmin()
        {
            //check if in the db theres an conection ,if not error then check if ther's an admin user if not create one
            try
            {
                UserDto user = await _userService.GetUserByUserNameAsync("admin");   
                if(user==null)
                { RoleDto role = await _roleService.GetRoleByIdAsync(6);
                    UserDto admin = new UserDto
                    {
                        Name = "admin",
                        Email = "Admin@email.com",
                        PhoneNumber = "123456789",
                        PasswordHashed = "admin", // it will be hashed in the service
                        Roles = new List<RoleDto> { role }
                    };

                   await _userService.CreateUserAsync(admin);

                    MessageBox.Show("An admin user has been created.\n\n" +
                                     "Username: admin\n" +
                                     "Password: admin\n\n" +
                                     "For security reasons, please change the password from the User Management tab.",
                                     "Admin User Created", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database. Please ensure that it is properly set up.",
                "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
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
