using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyBudget.UI.Core.Controls
{
    [ValueConversion(typeof(DateTime), typeof(string))] 
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //By default inproper CurrentUICulture is passed so we ignore culture parameter.
            //Should be CurrentCulture because DateTime setttings are in CurrentCulture setting.
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
