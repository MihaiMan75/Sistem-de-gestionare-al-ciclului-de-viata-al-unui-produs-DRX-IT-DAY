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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };
            mainWindow.Show();
        }
    }

}
