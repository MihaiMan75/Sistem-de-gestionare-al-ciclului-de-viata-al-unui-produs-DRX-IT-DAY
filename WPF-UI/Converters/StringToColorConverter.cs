using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_UI.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Green, Yellow,Orange,Red
            if (value.ToString() == "Green")
            {
                return "#00b894";
            }
            else if (value.ToString() == "Yellow")
            {
                return "#fdcb6e";
            }
            else if (value.ToString() == "Orange")
            {
                return "#e17055";
            }
            else if (value.ToString() == "Red")
            {
                return "#d63031";
            }
            else
            {
                return "#636e72";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
