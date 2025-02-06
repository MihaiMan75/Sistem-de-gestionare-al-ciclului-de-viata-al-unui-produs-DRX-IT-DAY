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

namespace WPF_UI.ViewModels
{

    public partial class LoginViewModel: BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        public LoginViewModel(INavigationService navigationService)
        {
            _authService = new AuthService();
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Login()
        {
            MessageBox.Show($"{Username} , {Password}");
            if (_authService.Login(Username, Password) || true)// Delete TRUE after Implemeting Proper Navigation
            {
                //navigate to the next page
                _navigationService.NavigateTo<TestPageViewModel>();

            }
            else
            {
                MessageBox.Show("Invalid credentials!");
            }
        }
    }
}
