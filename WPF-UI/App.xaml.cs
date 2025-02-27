using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using System.Configuration;
using System.Data;
using System.Windows;
using WPF_UI.Stores;
using WPF_UI.ViewModels;

namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore = new NavigationStore();
        private readonly IServiceFactory _serviceFactory;
        private readonly IDbContext _dbContext;

        public App()
        {
            _navigationStore = new NavigationStore();
            _dbContext = new DbContext("Data Source=LAPTOPDELL;Initial Catalog=DRXITDAY_TEST;Integrated Security=True;TrustServerCertificate=True"); // Momentarily hardcoded connection string
            _serviceFactory = new ServiceFactory(_dbContext);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(_navigationStore,_serviceFactory)
            };
            mainWindow.Show();
        }
    }

}
