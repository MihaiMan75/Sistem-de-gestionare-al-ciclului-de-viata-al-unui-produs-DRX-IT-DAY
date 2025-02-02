using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Interfaces;
using UI.Stores;

namespace UI.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private ObservableObject _currentViewModel;

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public void NavigateTo<T>() where T : ObservableObject, new()
        {
            CurrentViewModel = new T();
        }
    }
}
