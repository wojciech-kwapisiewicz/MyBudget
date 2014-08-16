using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyBudget.UI.Core.Controls
{
    [ValueConversion(typeof(decimal), typeof(string))] 
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //By default inproper CurrentUICulture is passed so we ignore culture parameter.
            //Should be CurrentCulture because monetary setttings are in CurrentCulture setting.
            decimal number = (decimal)value;
            string format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return number.ToString(format, CultureInfo.CurrentCulture);
            }
            else
            {
                //By default just date is returned
                return number.ToString(CultureInfo.CurrentCulture);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
            
}
