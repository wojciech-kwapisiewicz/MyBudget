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

namespace MyBudget.UI.Operations
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl, IRegionMemberLifetime
    {
        public StatisticsView()
        {
            InitializeComponent();
        }

        public StatisticsView(StatisticsViewModel viewModel)
        {           
            ViewModel = viewModel;
            InitializeComponent();
            Wrapper.DataContext = this;
        }

        public StatisticsViewModel ViewModel { get; set; }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
