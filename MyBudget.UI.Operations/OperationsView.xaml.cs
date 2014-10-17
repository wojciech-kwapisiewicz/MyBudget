using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;

namespace MyBudget.UI.Operations
{
    /// <summary>
    /// Interaction logic for OperationsView.xaml
    /// </summary>
    public partial class OperationsView : UserControl, IRegionMemberLifetime
    {
        public OperationsView()
        {

        }

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
