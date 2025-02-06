using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Interfaces;
using UI.Stores;
using UI.ViewModels;

namespace UI.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private BaseViewModel _currentViewModel;
        public readonly NavigationStore _navigationStore;

        [ObservableProperty]
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public NavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            CurrentViewModel = new LoginViewModel();
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }

        public void NavigateTo<T>() where T : BaseViewModel, new()
        {
            CurrentViewModel = new T();
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }
    }
}
