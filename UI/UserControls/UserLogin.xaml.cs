
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : UserControl
    {
       
        public UserLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
          //  string username = txtUsername.txtInput.Text;
          //  string password = cPassBox.Password;

          //  if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
          //  {
          //      MessageBox.Show("Please enter both username and password.", "Error");
          //      return;
          //  }
          //if(  MainWindow.viewControllerInstance.LogInUser(username, password))
          //  {
          //      ((MainWindow)Application.Current.MainWindow).MainFrame.Content = new Pages.MainPage();
          //  }
          //  else
          //  {
          //      MessageBox.Show("Try again!", "Error");
          //  }
            
        }
        
    }
}
