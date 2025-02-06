using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : ObservableObject, new();
        //go back method
        //go foward?
    }
}
