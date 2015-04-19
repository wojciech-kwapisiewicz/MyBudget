using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MyBudget.UI.Core
{
    public class MultiSelectedBehaviour : Behavior<System.Windows.Controls.Primitives.Selector>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
        }

        void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var multiSelector = AssociatedObject as System.Windows.Controls.Primitives.MultiSelector;
            var listBox = AssociatedObject as ListBox;
            if (multiSelector != null)
            {
                SelectedItems = multiSelector.SelectedItems;
            }
            else if(listBox!=null)
            {
                //    var array = new object[listBox.SelectedItems.Count];
                //    listBox.SelectedItems.CopyTo(array, 0);
                //SelectedItems = array;
                SelectedItems = listBox.SelectedItems;
            }
            else
            {
                throw new NotSupportedException(string.Format("Type of {0} is not supported for multiselection", AssociatedObject.GetType()));
            }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(MultiSelectedBehaviour),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
    }
}
