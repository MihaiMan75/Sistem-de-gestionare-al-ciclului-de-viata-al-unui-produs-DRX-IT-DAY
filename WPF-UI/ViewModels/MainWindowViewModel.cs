using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.Interfaces;
using WPF_UI.Messages;
using WPF_UI.Services;
using WPF_UI.Stores;

namespace WPF_UI.ViewModels
{
    public partial class MainWindowViewModel: BaseViewModel, IRecipient<UserChangedMessage>
    {
        private readonly NavigationStore _navigationStore;
        private readonly INavigationService _navigationService;
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private BaseViewModel _currentViewModel;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _userRole;

        [ObservableProperty]
        private bool _isLogged = false;

        [ObservableProperty]
        private bool _allowedToManageProducts = false;
        [ObservableProperty]
        private bool _allowedToManageMaterials = false;
        [ObservableProperty]
        private bool _allowedToManageUsers = false;


        public MainWindowViewModel(NavigationStore NavigationStore,IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _authService = new AuthService(serviceFactory);
            _navigationStore = NavigationStore;
            _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
            CurrentViewModel = _navigationStore.CurrentViewModel;
            _navigationService = new NavigationService(_navigationStore,_serviceFactory, _authService);
            WeakReferenceMessenger.Default.Register<UserChangedMessage>(this);
            UpdateUserInfo(null);
        }

        


        private void UpdateUserInfo(UserDto user)
        {
            ResesetAllowedActions();
            if (user != null)
            {
                UserName = user.Name;
                UserRole = "";
                var roles = user.Roles;
                foreach (var role in roles)
                {
                    UserRole += role.RoleName + ", ";
                }
                UserRole = UserRole.Substring(0, UserRole.Length - 2);
                IsLogged = true;
                //depeing of the roles we can set the allowed actions
                RefreshAllowedActions(roles);
            }
            else
            {
                UserName = "Not Logged in";
                UserRole = "None";
                IsLogged = false;
            }
        }

        private void RefreshAllowedActions(List<RoleDto> roles)
        {
            var adminRoleId = 6;
            var productRoles = new List<int> { 1, 2, 3 };
            var materialRoles = new List<int> { 2, 4 };

            foreach (var role in roles)
            {
                switch (role.Id)
                {
                    case 6: // Admin
                        AllowedToManageProducts = true;
                        AllowedToManageMaterials = true;
                        AllowedToManageUsers = true;
                        return;
                    default:
                        if (productRoles.Contains(role.Id)) AllowedToManageProducts = true;
                        if (materialRoles.Contains(role.Id)) AllowedToManageMaterials = true;
                        break;
                }

            }
        }

        private void ResesetAllowedActions()
        {
            AllowedToManageProducts = false;
            AllowedToManageMaterials = false;
            AllowedToManageUsers = false;
        }

        private void OnCurrentViewModelChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
        }

        [RelayCommand]
        public void NavigateHome()
        {
            _navigationService.NavigateTo<UserDashboardViewModel>();
        }

        [RelayCommand]
        public void NavigateProductManagement()
        {
            _navigationService.NavigateTo<ProductManagementViewModel>();
        }

        [RelayCommand]
        public void NavigateProductDetails()
        {
            _navigationService.NavigateTo<ProductDetailsViewModel>();
        }

        [RelayCommand]
        public void NavigateBOMManagement()
        {
            _navigationService.NavigateTo<BOMManagementViewModel>();
        }

        [RelayCommand]
        public void NavigateUserManagement()
        {
            _navigationService.NavigateTo<UserManagementViewModel>();
        }

        [RelayCommand]
        public void NavigateReports()
        {
            _navigationService.NavigateTo<ReportsViewModel>();
        }

        [RelayCommand]
        public void NavigateMaterialsManagement()
        {
            _navigationService.NavigateTo<MaterialManagementViewModel>();
        }
        [RelayCommand]
        public void LogOut()
        {
            
            _navigationService.NavigateToLogin();
        }

        public void Receive(UserChangedMessage message)
        {
            UpdateUserInfo(message.User);
        }
    }
}
