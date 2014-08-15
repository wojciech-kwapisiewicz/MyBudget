using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core;
using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MyBudget.UI.Accounts
{
    /// <summary>
    /// Interaction logic for OperationsView.xaml
    /// </summary>
    public partial class OperationsView : UserControl, IRegionMemberLifetime
    {
        public OperationsView(OperationsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public OperationsViewModel ViewModel { get; set; }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
