using System.Windows.Controls;

namespace MyBudget.UI.Accounts
{
    /// <summary>
    /// Interaction logic for StatementsView.xaml
    /// </summary>
    public partial class StatementsView : UserControl
    {
        public StatementsView()
        {

        }

        public StatementsView(StatementsViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            Wrapper.DataContext = viewModel;
        }

        public StatementsViewModel ViewModel { get; set; }
    }
}
