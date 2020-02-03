using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace WaterAndWastewaterAuthorithy.BindingConvertor
{
    class BoolConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            if (b == true)
                return "بلی";
            else
                return "خیر";
        }
        public object ConvertBack(object value, Type targetType,object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            if (b == true)
                return "بلی";
            else
                return "خیر";            
        }
    }
}
