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
    /// Interaction logic for MenuButton.xaml
    /// </summary>
    public partial class MenuButton : UserControl
    {
        public MenuButton()
        {
            InitializeComponent();
        }

        public ICommand MenuCommand
        {
            get { return (ICommand)GetValue(MenuCommandProperty); }
            set { SetValue(MenuCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuCommandProperty =
            DependencyProperty.Register("MenuCommand", typeof(ICommand), typeof(MenuButton), new PropertyMetadata(null));
       
        public string MenuText
        {
            get { return (string)GetValue(MenuTextProperty); }
            set { SetValue(MenuTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuTextProperty =
            DependencyProperty.Register("MenuText", typeof(string), typeof(MenuButton), new PropertyMetadata(string.Empty));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MenuButton), new PropertyMetadata(null));       
    }
}
