using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyBudget.UI.Core.Popups
{
    public class ButtonsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MessageBoxButton supportedButtons = (MessageBoxButton)value;
            MessageBoxResult resultButton = (MessageBoxResult)parameter;

            bool shouldBeVisible = false;
            switch (resultButton)
            {
                case MessageBoxResult.Cancel:
                    if (supportedButtons == MessageBoxButton.OKCancel ||
                        supportedButtons == MessageBoxButton.YesNoCancel)
                    {
                        shouldBeVisible = true;
                    }
                    break;
                case MessageBoxResult.No:
                    if (supportedButtons == MessageBoxButton.YesNo ||
                        supportedButtons == MessageBoxButton.YesNoCancel)
                    {
                        shouldBeVisible = true;
                    }
                    break;
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    if (supportedButtons == MessageBoxButton.OK ||
                        supportedButtons == MessageBoxButton.OKCancel)
                    {
                        shouldBeVisible = true;
                    }
                    break;
                case MessageBoxResult.Yes:
                    if (supportedButtons == MessageBoxButton.YesNo ||
                        supportedButtons == MessageBoxButton.YesNoCancel)
                    {
                        shouldBeVisible = true;
                    }
                    break;
                default:
                    break;
            }

            return shouldBeVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
