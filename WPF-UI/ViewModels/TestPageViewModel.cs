using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.Interfaces;
using WPF_UI.Services;

namespace WPF_UI.ViewModels
{
    public class TestPageViewModel : BaseViewModel
    {
        private readonly IServiceFactory _serviceFactory;
        public TestPageViewModel(IServiceFactory serviceFactory, IAuthService authService)
        {
            _serviceFactory = serviceFactory;
        }
    }
}
