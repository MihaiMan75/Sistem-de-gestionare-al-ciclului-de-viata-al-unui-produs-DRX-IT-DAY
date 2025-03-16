using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
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
        private string _connectionString = "";
        private const string ConfigFileName = "connectionSettings.json";

        public App()
        {
            _connectionString = LoadConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                MessageBox.Show("The connection settings file ('connectionSettings.json') is missing or empty. " +
                "A template file has been created in the Release/Debug directory. " +
                "Please update it with the correct connection string and restart the application. " +
                "Also, ensure that the database is properly set up.",
                "Missing Connection String", MessageBoxButton.OK, MessageBoxImage.Warning);

                //Application.Current.Shutdown(); 
                return;
            }
                _navigationStore = new NavigationStore();
                _dbContext = new DbContext(_connectionString);
                _serviceFactory = new ServiceFactory(_dbContext);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                Application.Current.Shutdown();
                return;
            }
            if (!_connectionString.Contains("TrustServerCertificate=True", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The connection string does not contain 'TrustServerCertificate=True' (without any spaces).\n" +
                "This setting is required to connect to a local SQL Server instance.\n\n" +
                "Please add it to the connection string and restart the application.",
                "Missing Connection String Setting", MessageBoxButton.OK, MessageBoxImage.Error);

                //Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(_navigationStore,_serviceFactory)
            };
            mainWindow.Show();
        }

        private string LoadConnectionString()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);

            if (!File.Exists(filePath))
            {
                var defaultConfig = new { ConnectionString = "" };
                File.WriteAllText(filePath, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
                return null;
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                var config = JsonSerializer.Deserialize<ConnectionConfig>(jsonContent);
                return config?.ConnectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading the connection settings file: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        private class ConnectionConfig
        {
            public string ConnectionString { get; set; }
        }
    }


}
