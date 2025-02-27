using BusinessLogic.DtoModels;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.Interfaces;
using WPF_UI.Services;
using WPF_UI.Stores;

namespace WPF_UI.ViewModels
{
    public partial class MainWindowViewModel: BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly INavigationService _navigationService;
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private BaseViewModel _currentViewModel;




        public MainWindowViewModel(NavigationStore NavigationStore,IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _authService = new AuthService(serviceFactory);
            _navigationStore = NavigationStore;
            _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
            CurrentViewModel = _navigationStore.CurrentViewModel;
            _navigationService = new NavigationService(_navigationStore,_serviceFactory, _authService);

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


    }
}
