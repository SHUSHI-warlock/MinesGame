using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MinesGame.Converter
{
    class IsOpenConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            char flag = (char)value;

            //保留两位转换
            return flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            double price;
            if (double.TryParse(strValue, out price))
            {
                //返回整数
                return (int)(price * 100);
            }
            return value;
        }
    }
}
