﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.ViewModels;

namespace WPF_UI.Interfaces
{
    public interface INavigationService
    {
        void NavigateToSimple<T>() where T : BaseViewModel, new();
        void NavigateTo<T>() where T : BaseViewModel;
        void NavigateBack();
        void NavigateToLogin();
       
    }
}
