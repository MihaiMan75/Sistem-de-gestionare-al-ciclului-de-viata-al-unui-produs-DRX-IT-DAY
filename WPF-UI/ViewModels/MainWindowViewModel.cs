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
            }
            else
            {
                UserName = "Not Logged in";
                UserRole = "None";
            }
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
