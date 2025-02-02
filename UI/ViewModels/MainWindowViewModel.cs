using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Services;
using UI.Stores;

namespace UI.ViewModels
{
    public class MainWindowViewModel: ObservableObject
    {
        public NavigationService Navigation { get; }

        public RelayCommand NavigateToLoginCommand { get; }
        public RelayCommand NavigateToTestCommand { get; }

        public MainWindowViewModel(NavigationService navigationService)
        {
            Navigation = navigationService;

            NavigateToLoginCommand = new RelayCommand(() => Navigation.NavigateTo<LoginViewModel>());
            NavigateToTestCommand = new RelayCommand(() => Navigation.NavigateTo<TestPageViewModel>());

            // Default Page
            Navigation.NavigateTo<LoginViewModel>();
        }

    }
}
