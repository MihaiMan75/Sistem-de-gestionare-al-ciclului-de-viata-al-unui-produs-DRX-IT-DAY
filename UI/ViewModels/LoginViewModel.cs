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
using UI.Interfaces;
using UI.Services;

namespace UI.ViewModels
{
    [ObservableObject]
    public partial class LoginViewModel
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
