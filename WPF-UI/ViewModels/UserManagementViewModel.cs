using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManagementBusinessLogic.AuthUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF_UI.Interfaces;
using WPF_UI.Wrappers;

namespace WPF_UI.ViewModels
{
    public partial class UserManagementViewModel : BaseViewModel
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IPasswordHasher PasswordHasher = new PasswordHasher();
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        [ObservableProperty]
        private ObservableCollection<UserDto> _users;

        [ObservableProperty]
        private UserDto _selectedUser;

        [ObservableProperty]
        private UserDto _currentUser;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<RoleSelectionWrap> _availableRoles;


        public UserManagementViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
            _userService = _serviceFactory.GetUserService();
            _roleService = _serviceFactory.GetRoleService();
            CurrentUser = new UserDto();
            LoadUsersCommand.Execute(null);
            LoadRolesCommand.Execute(null);
            
        }

        [RelayCommand]
        private async Task LoadUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                Users = new ObservableCollection<UserDto>(users);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading Users: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task LoadRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                AvailableRoles = new ObservableCollection<RoleSelectionWrap>(
                    roles.Select(role => new RoleSelectionWrap(role))
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading Roles: {ex.Message}");
            }
        }

        public List<RoleDto> GetSelectedRoles()
        {
            return AvailableRoles.Where(r => r.IsSelected).Select(r => r.Role).ToList();
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LoadUsersCommand.Execute(null);
                return;
            }

            var filteredList = Users.Where(u =>
         u.Name.Contains(value, StringComparison.OrdinalIgnoreCase) ||
         u.Roles.Any(role => role.RoleName.Contains(value, StringComparison.OrdinalIgnoreCase))
            );


            Users = new ObservableCollection<UserDto>(filteredList);
        }



        [RelayCommand]
        private async Task Save()
        {
            //add active roles to curent user
            CurrentUser.Roles = GetSelectedRoles();

            if(CurrentUser.Roles.Count == 0)
            {
                MessageBox.Show("Please select at least one role", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            try
            {
                var existingUser = await _userService.GetUserByIdAsync(CurrentUser.Id);

                if (existingUser != null)
                {
                    existingUser.Id = CurrentUser.Id;
                    existingUser.Name = CurrentUser.Name;
                    existingUser.PhoneNumber = CurrentUser.PhoneNumber;
                    existingUser.Email= CurrentUser.Email;
                    existingUser.Roles = CurrentUser.Roles;
                    existingUser.PasswordHashed = PasswordHasher.HashPassword(CurrentUser.PasswordHashed);
                    await _userService.UpdateUserAsync(existingUser);

                }
                else
                {
                    await _userService.CreateUserAsync(CurrentUser);
                }

                await LoadUsers();
                ResetForm();
            }
            catch (Exception ex)
            {
               MessageBox.Show($"Error saving user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

            [RelayCommand]
        private void Cancel()
        {
            ResetForm();
        }

        [RelayCommand]
        private void Edit(UserDto user)
        {
            if (user == null) return;

            // Create a copy of the material for editing
            CurrentUser = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles,
                PasswordHashed = ""
            };
            //also set roles in the ui
            foreach (var role in AvailableRoles)
            {
                role.IsSelected = CurrentUser.Roles.Any(r => r.Id == role.Role.Id);
            }
        }

        [RelayCommand]
        private async Task Delete(UserDto user)
        {
            if (user == null) return;

            try
            {
                // Could add confirmation dialog here
              var answ = MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answ == MessageBoxResult.No)
                {
                    return;
                }

                await _userService.DeleteUserAsync(user.Id);
                await LoadUsers();
            }
            catch (Exception ex)
            {
                // Handle error - could show message to user
                System.Diagnostics.Debug.WriteLine($"Error deleting material: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            CurrentUser = new UserDto();
            foreach (var role in AvailableRoles)
            {
                role.IsSelected = false;
            }
            SelectedUser = null;
        }

    }
}
