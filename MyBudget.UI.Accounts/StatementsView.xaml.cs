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
            DataContext = this;
            InitializeComponent();
        }

        public StatementsViewModel ViewModel { get; set; }
    }
}
