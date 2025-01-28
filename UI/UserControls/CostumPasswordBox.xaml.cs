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
    /// Interaction logic for CostumPasswordBox.xaml
    /// </summary>
    public partial class CostumPasswordBox : UserControl
    {
        public CostumPasswordBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(CostumPasswordBox),
                new PropertyMetadata(string.Empty, OnPasswordChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CostumPasswordBox)d;
            control.UpdatePasswordFields((string)e.NewValue);
        }

        private void UpdatePasswordFields(string password)
        {
            if (pwdHiddenPassword.Visibility == Visibility.Visible && pwdHiddenPassword.Password != password)
            {
                pwdHiddenPassword.Password = password;
            }

            if (txtVisiblePassword.Visibility == Visibility.Visible && txtVisiblePassword.Text != password)
            {
                txtVisiblePassword.Text = password;
            }
        }

        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (pwdHiddenPassword.Visibility == Visibility.Visible)
            {
                txtVisiblePassword.Text = pwdHiddenPassword.Password;
                pwdHiddenPassword.Visibility = Visibility.Collapsed;
                txtVisiblePassword.Visibility = Visibility.Visible;
            }
            else
            {
                pwdHiddenPassword.Password = txtVisiblePassword.Text;
                txtVisiblePassword.Visibility = Visibility.Collapsed;
                pwdHiddenPassword.Visibility = Visibility.Visible;
            }
        }

        private void btnClearPassword_Click(object sender, RoutedEventArgs e)
        {
            txtVisiblePassword.Clear();
            pwdHiddenPassword.Clear();
            Password = string.Empty;
        }

        private void pwdHiddenPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwdHiddenPassword.Visibility == Visibility.Visible)
            {
                Password = pwdHiddenPassword.Password;
            }
        }

        private void txtVisiblePassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtVisiblePassword.Visibility == Visibility.Visible)
            {
                Password = txtVisiblePassword.Text;
            }
        }
    }
}
