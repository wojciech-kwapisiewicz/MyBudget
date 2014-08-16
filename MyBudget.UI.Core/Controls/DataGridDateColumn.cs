using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyBudget.UI.Core.Controls
{
    public class DataGridDateColumn : DataGridBoundColumn
    {
        protected override System.Windows.FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            throw new NotImplementedException();
        }

        protected override System.Windows.FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var text = new TextBlock();
            text.TextAlignment = System.Windows.TextAlignment.Right;
            text.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            text.MinWidth = 70;
            var currentBinding = this.Binding as Binding;
            var bindingToSet = new Binding
            {
                Path = currentBinding.Path,
                Source = cell.DataContext,
                Converter = new DateToStringConverter()
            };
            text.SetBinding(TextBlock.TextProperty, bindingToSet);
            return text;
        }
    }
}
