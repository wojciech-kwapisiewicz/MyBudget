using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
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

namespace MyBudget.UI.Main
{
    /// <summary>
    /// Interaction logic for WelcomePageView.xaml
    /// </summary>
    public partial class WelcomePageView : UserControl, IRegionMemberLifetime
    {
        public WelcomePageView()
        {
            ShowStandardMsgBox = new DelegateCommand(() => MessageBox.Show("Windows"));
            ShowToolkitMsgBox = new DelegateCommand(ShowToolkig);
            
            this.DataContext = this;
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }

        private object _Selected;
        public object Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }

        public ICommand ShowStandardMsgBox { get; set; }
        public ICommand ShowToolkitMsgBox { get; set; }

        void ShowToolkig()
        {
            System.Windows.Style style = new System.Windows.Style();
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.YesButtonContentProperty, "Yes, FTW!"));
            style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.NoButtonContentProperty, "Omg, no"));

            Xceed.Wpf.Toolkit.MessageBox.Show("a", "b", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No, style);
        }
    }
}
