﻿using System;
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

namespace WPF_UI.UserControls
{
    /// <summary>
    /// Interaction logic for ForgotPasswordTextBlock.xaml
    /// </summary>
    public partial class ForgotPasswordTextBlock : UserControl
    {
        public ForgotPasswordTextBlock()
        {
            InitializeComponent();
        }
        private void txtForgotPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popupForgotPassword.IsOpen = true;
        }
    }
}
