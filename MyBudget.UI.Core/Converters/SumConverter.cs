using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyBudget.UI.Core.Converters
{
    public class SumConverter : IValueConverter
    {
        DecimalToStringConverter _converter = new DecimalToStringConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<object> en = value as IEnumerable<object>;
            if(en==null)
            {
                return DependencyProperty.UnsetValue;
            }
            else
            {
                decimal sum = en.OfType<BankOperation>().Sum(a => a.Amount);
                return _converter.Convert(sum, targetType, parameter, culture);
            }          
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
