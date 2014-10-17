using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using MyBudget.UI.Accounts;
using MyBudget.UI.Core;
using MyBudget.UI.Operations;
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

            NavigateWelcomePage = new DelegateCommand(() => 
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(WelcomePageView).FullName));

            NavigateAccounts = new DelegateCommand(() => 
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(AccountsView).FullName));

            NavigateStatements = new DelegateCommand(() =>
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(StatementsView).FullName));

            NavigateOperations = new DelegateCommand(() =>
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(OperationsView).FullName));

            NavigateLanguageSetup = new DelegateCommand(() =>
    regionManager.RequestNavigate(RegionNames.MainContent, typeof(LanguageSetupView).FullName));

            InitializeComponent();
        }

        public ICommand NavigateWelcomePage { get; set; }
        public ICommand NavigateAccounts { get; set; }
        public ICommand NavigateStatements { get; set; }
        public ICommand NavigateOperations { get; set; }
        public ICommand NavigateLanguageSetup { get; set; }
    }
}
