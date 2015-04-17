using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyBudget.UI.Core.Converters
{
    public class ComboBoxDisableSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ILabel)
            {
                return ((ILabel)value).Text;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ISelectionMarker ci = value as ISelectionMarker;
            if (ci != null && ci.IsSelectable)
            {
                if (value is ILabel)
                {
                    return ((ILabel)value).Text;
                }
                return ci;
            }

            return DependencyProperty.UnsetValue;
        }
    }

    public interface ILabel
    {
        string Text { get; }
    }

    public class SelectableText : ISelectionMarker, ILabel
    {
        public bool IsSelectable
        {
            get { return true; }
        }

        public string Text
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class StaticText : ISelectionMarker, ILabel
    {
        public bool IsSelectable
        {
            get { return false; }
        }

        public string Text
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
