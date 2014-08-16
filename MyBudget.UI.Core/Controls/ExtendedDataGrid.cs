using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBudget.UI.Core.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyBudget.UI.Core.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyBudget.UI.Core.Controls;assembly=MyBudget.UI.Core.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ExtendedDataGrid/>
    ///
    /// </summary>
    public class ExtendedDataGrid : DataGrid
    {
        static ExtendedDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedDataGrid), new FrameworkPropertyMetadata(typeof(ExtendedDataGrid)));
        }

        public LocalizedReflection LocalizedReflection
        {
            get { return (LocalizedReflection)GetValue(LocalizedReflectionProperty); }
            set { SetValue(LocalizedReflectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocalizedReflection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalizedReflectionProperty =
            DependencyProperty.Register("LocalizedReflection", typeof(LocalizedReflection), typeof(ExtendedDataGrid), new PropertyMetadata(null, SetUpColumns));

        public Type GridForType
        {
            get { return (Type)GetValue(GridForTypeProperty); }
            set { SetValue(GridForTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridForType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridForTypeProperty =
            DependencyProperty.Register("GridForType", typeof(Type), typeof(ExtendedDataGrid), new PropertyMetadata(typeof(object), SetUpColumns));

        private static void SetUpColumns(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (d as ExtendedDataGrid);
            grid.Columns.Clear();

            var properties = grid.GridForType.GetProperties().FilterOutDontDisplay();

            foreach (var property in properties)
            {
                string header =
                    grid.LocalizedReflection != null ?
                    grid.LocalizedReflection.GetPropertyName(property) :
                    property.Name;

                DataGridColumn column = GetColumn(property, header);
                grid.Columns.Add(column);
            }
        }

        private static DataGridColumn GetColumn(System.Reflection.PropertyInfo property, string header)
        {
            DataGridColumn column;
            if (property.PropertyType == typeof(DateTime))
            {
                column = new DataGridDateColumn()
                {
                    Header = header,
                    Binding = new Binding(property.Name),
                    MinWidth = 70,
                };
            }
            else if (property.PropertyType == typeof(decimal))
            {
                column = new DataGridDecimalColumn()
                {
                    Header = header,
                    Binding = new Binding(property.Name),
                    MinWidth = 110,
                };
            }
            else
            {
                column = new DataGridTextColumn()
                {
                    Header = header,
                    Binding = new Binding(property.Name)
                };
            }
            return column;
        }
    }
}
