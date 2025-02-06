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

        [ObservableProperty]
        private BaseViewModel _currentViewModel;

        public RelayCommand NavigateToLoginCommand { get; }
        public RelayCommand NavigateToTestCommand { get; }
        

        public MainWindowViewModel(NavigationStore NavigationStore)
        {
            this._navigationStore = NavigationStore;
            _navigationService = new NavigationService(this._navigationStore);
            CurrentViewModel = _navigationStore?.CurrentViewModel;

        }

    }
}
