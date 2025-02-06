using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.Interfaces;
using WPF_UI.Stores;
using WPF_UI.ViewModels;

namespace WPF_UI.Services
{

    public partial class NavigationService : ObservableObject, INavigationService
    {
        [ObservableProperty]
        private BaseViewModel _currentViewModel;

        private readonly NavigationStore _navigationStore;


        public NavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            CurrentViewModel = new LoginViewModel(this);
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }

        public void NavigateTo<T>() where T : BaseViewModel, new()
        {
            CurrentViewModel = new T();
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }
    }
}
