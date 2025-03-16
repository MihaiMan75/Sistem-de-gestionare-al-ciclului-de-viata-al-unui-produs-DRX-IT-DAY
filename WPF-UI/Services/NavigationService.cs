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
        [ObservableProperty]
        private BaseViewModel _previousViewModel;

        private readonly NavigationStore _navigationStore;
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuthService _authService;



        public NavigationService(NavigationStore navigationStore, IServiceFactory serviceFactory,IAuthService authService)
        {
            _navigationStore = navigationStore;
            _authService = authService;
            NavigateToLogin();
            _navigationStore.CurrentViewModel = CurrentViewModel;
            _navigationStore.PreviousViewModel = _previousViewModel;
            _serviceFactory = serviceFactory;
        }

        public void NavigateToSimple<T>() where T : BaseViewModel, new()
        {
            PreviousViewModel = CurrentViewModel;
            CurrentViewModel = new T();
            _navigationStore.PreviousViewModel = PreviousViewModel;
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }

        public void NavigateTo<T>() where T : BaseViewModel
        {
            // Activator create instace for instacianting with parameters
            PreviousViewModel = CurrentViewModel;
            CurrentViewModel = (T)Activator.CreateInstance(typeof(T), _serviceFactory,_authService,this);
            _navigationStore.PreviousViewModel = PreviousViewModel;
            _navigationStore.CurrentViewModel = CurrentViewModel;
        }

        public void NavigateToLogin()
        { 
            _authService.Logout();
            CurrentViewModel = new LoginViewModel(this, _authService);
            PreviousViewModel = CurrentViewModel;
            _navigationStore.CurrentViewModel = CurrentViewModel;
            _navigationStore.PreviousViewModel = PreviousViewModel;
        }
        public void NavigateBack()
        {
            if (PreviousViewModel != null)
            {
                var temp = CurrentViewModel;
                CurrentViewModel = PreviousViewModel;
                _navigationStore.CurrentViewModel = CurrentViewModel;
                PreviousViewModel = temp;
                _navigationStore.PreviousViewModel = PreviousViewModel;
                if(CurrentViewModel is UserDashboardViewModel)
                {
                    ((UserDashboardViewModel)CurrentViewModel).LoadProductsCommand.Execute(null);
                }
                else if(CurrentViewModel is ProductManagementViewModel)
                {
                    ((ProductManagementViewModel)CurrentViewModel).LoadProductsCommand.Execute(null);
                }
            }
        }
    }
}
