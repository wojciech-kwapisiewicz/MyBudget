using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyBudget.UI.Core.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class FixedUiToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //By default inproper CurrentUICulture is passed so we ignore culture parameter.
            //Should be CurrentCulture because monetary setttings are in CurrentCulture setting.
            if (value == null)
                return null;
            Type type = value.GetType();
            if (type == typeof(decimal))
            {
                return ConvertDecimal(value, parameter);
            }
            if (type == typeof(DateTime))
            {
                return ConvertDateTime(value, parameter);
            }
            return value.ToString();
        }

        private static object ConvertDateTime(object value, object parameter)
        {
            DateTime date = (DateTime)value;
            string format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return date.ToLocalTime().ToString(format, CultureInfo.CurrentCulture);
            }
            else
            {
                //By default just date is returned
                return date.ToString("d", CultureInfo.CurrentCulture);
            }
        }

        private static object ConvertDecimal(object value, object parameter)
        {
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
