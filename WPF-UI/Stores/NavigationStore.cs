using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WPF_UI.ViewModels;

namespace WPF_UI.Stores
{

    /// <summary>
    /// Represents a class that stores the current view model data.
    /// </summary>
    public partial class NavigationStore : ObservableObject
    {
        [ObservableProperty]
        private BaseViewModel _currentViewModel;

    }
}
