using BusinessLogic;
using BusinessLogic.Interfaces;
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
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuthService _authService;



        public NavigationService(NavigationStore navigationStore, IServiceFactory serviceFactory,IAuthService authService)
        {
            _navigationStore = navigationStore;
            _authService = authService;
            CurrentViewModel = new LoginViewModel(this,authService);
            _navigationStore.CurrentViewModel = CurrentViewModel;
            _serviceFactory = serviceFactory;
        }

        public void NavigateToSimple<T>() where T : BaseViewModel, new()
        {
            CurrentViewModel = new T();
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }

        public void NavigateTo<T>() where T : BaseViewModel
        {
            // Activator create instace for instacianting with parameters
            CurrentViewModel = (T)Activator.CreateInstance(typeof(T), _serviceFactory,_authService);
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }
    }
}
