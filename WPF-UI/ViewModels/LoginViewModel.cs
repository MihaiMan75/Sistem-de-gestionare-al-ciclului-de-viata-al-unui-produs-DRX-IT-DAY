using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using WPF_UI.Interfaces;
using WPF_UI.Services;
using BusinessLogic.Interfaces;

namespace WPF_UI.ViewModels
{

    public partial class LoginViewModel: BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IServiceFactory _serviceFactory;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        public LoginViewModel(INavigationService navigationService, IAuthService authService )
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        public async Task Login()
        {
            if (await _authService.Login(Username, Password))// Delete TRUE after Implemeting Proper Navigation
            {
                //navigate to the next page

                _navigationService.NavigateTo<UserDashboardViewModel>();

            }
            else
            {
                MessageBox.Show("Invalid credentials!");
            }
        }
    }
}
