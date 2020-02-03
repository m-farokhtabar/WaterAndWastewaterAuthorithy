using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace WaterAndWastewaterAuthorithy.BindingConvertor
{
    class MoneyConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long b = (long)value;
            b = b * -1;
            return b;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long b = (long)value;
            b = b * -1;
            return b;
        }
    }
}