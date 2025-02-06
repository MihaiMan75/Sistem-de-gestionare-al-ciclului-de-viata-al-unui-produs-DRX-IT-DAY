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
        private readonly IAuthService authService;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        public LoginViewModel()
        {
            authService = new AuthService();
        }

        [RelayCommand]
        public void Login()
        {
            MessageBox.Show($"{Username} , {Password}");
            if (authService.Login(Username, Password))
            {
                //navigate to the next page
            }
            else
            {
                MessageBox.Show("Invalid credentials!");
            }
        }
    }
}
