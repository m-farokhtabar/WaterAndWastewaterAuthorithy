using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using WaterAndWastewaterAuthorithy.Presentation;

namespace WaterAndWastewaterAuthorithy.BindingConvertor
{
    class StatusConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BillsStatus b = (BillsStatus)value;
            if (b == BillsStatus.Billing)
                return "صادر شده";
            else
                if (b == BillsStatus.Cancel)
                    return "ابطال شده";
                else                    
                   return "پرداخت شده";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BillsStatus b = (BillsStatus)value;
            if (b == BillsStatus.Billing)
                return "صادر شده";
            else
                if (b == BillsStatus.Cancel)
                    return "ابطال شده";
                else
                    return "پرداخت شده";
        }
    }
}
