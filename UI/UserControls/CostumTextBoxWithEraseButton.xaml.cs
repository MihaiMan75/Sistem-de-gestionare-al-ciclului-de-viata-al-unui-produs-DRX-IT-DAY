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
    /// Interaction logic for CostumTextBoxWithEraseButton.xaml
    /// </summary>
    public partial class CostumTextBoxWithEraseButton : UserControl
    {

        private string placeHolder;

        public string PlaceHodler
        {
            get { return placeHolder; }
            set
            {
                placeHolder = value;
                tbPlaceHolder.Text = placeHolder;
            }
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register(nameof(Text), typeof(string), typeof(CostumTextBoxWithEraseButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public CostumTextBoxWithEraseButton()
        {
            InitializeComponent();
            txtInput.TextChanged += (s, e) => Text = txtInput.Text;
        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                tbPlaceHolder.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceHolder.Visibility = Visibility.Hidden;
            }
        }
    }
}
