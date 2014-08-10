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
    /// Interaction logic for MainNavigationView.xaml
    /// </summary>
    public partial class MainNavigationView : UserControl
    {
        public MainNavigationView(IRegionManager regionManager)
        {
            DataContext = this;

            NavigateOperations = new DelegateCommand(() =>
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(MainContentView).ToString()));

            NavigateWelcomePage = new DelegateCommand(() => 
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(WelcomePageView).ToString()));
            
            InitializeComponent();
        }


        public ICommand NavigateWelcomePage { get; set; }
        public ICommand NavigateOperations { get; set; }
    }
}
