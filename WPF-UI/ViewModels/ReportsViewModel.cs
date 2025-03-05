using BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.Interfaces;

namespace WPF_UI.ViewModels
{
     public class ReportsViewModel:BaseViewModel
    {
        private readonly IServiceFactory _serviceFactory;
        public ReportsViewModel(IServiceFactory serviceFactory, IAuthService authService, INavigationService navigationService)
        {
            _serviceFactory = serviceFactory;
        }
    }
}
