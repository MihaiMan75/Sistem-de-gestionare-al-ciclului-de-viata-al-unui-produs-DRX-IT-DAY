using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Interfaces;
using UI.Services;
using UI.Stores;

namespace UI.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        public BaseViewModel CurrentViewModel => _navigationStore?.CurrentViewModel;

        public RelayCommand NavigateToLoginCommand { get; }
        public RelayCommand NavigateToTestCommand { get; }
        

        public MainWindowViewModel(NavigationStore NavigationStore)
        {
            this._navigationStore = NavigationStore;
            _navigationService = new NavigationService(this._navigationStore);
              
        }

    }
}
