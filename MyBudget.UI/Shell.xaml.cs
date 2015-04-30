using System.Windows;

namespace MyBudget.UI
{
    /// <summary>
    /// Interaction logic for MainWindowShell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public AggregationViewModel ViewModel { get; set; }

        public Shell(AggregationViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
