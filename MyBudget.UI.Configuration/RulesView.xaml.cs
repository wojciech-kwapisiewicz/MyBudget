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

namespace MyBudget.UI.Configuration
{
    /// <summary>
    /// Interaction logic for RulesView.xaml
    /// </summary>
    public partial class RulesView : UserControl, IRegionMemberLifetime
    {
        public RulesViewModel ViewModel { get; set; }

        public RulesView(RulesViewModel viewModel)
        {
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
