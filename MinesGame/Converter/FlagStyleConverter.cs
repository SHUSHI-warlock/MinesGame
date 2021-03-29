using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MinesGame.Converter
{
    class FlagStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            char flag = (char)value;
            switch (flag) {
                case '0': return Brushes.Black;
                case '1': return Brushes.Blue;
                case '2': return Brushes.Green;
                case '3': return Brushes.OrangeRed;
                case '4': return Brushes.DarkBlue;
                case '5': return Brushes.DarkRed;
                case '6': return Brushes.LightGreen;
                case '7': return Brushes.Black;
                case '8': return Brushes.Gray;
                case '*': return Brushes.Red;
                default:
                    return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
