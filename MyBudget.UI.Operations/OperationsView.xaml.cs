using Microsoft.Practices.Prism.Regions;
using MyBudget.UI.Operations.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyBudget.UI.Operations
{
    /// <summary>
    /// Interaction logic for OperationsView.xaml
    /// </summary>
    public partial class OperationsView : UserControl, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        public OperationsView()
        {

        }

        public OperationsView(OperationsViewModel viewModel)
        {         
            ViewModel = viewModel;
            viewModel.OnNextSelected = () =>
                {
                    var operationCategoryTextBox = OperationCategoryElement.Template
                        .FindName("Text", OperationCategoryElement) as TextBox;
                    operationCategoryTextBox.SelectAll();
                    operationsGrid.ScrollIntoView(viewModel.SelectedOperation);
                };
            InitializeComponent();
            Wrapper.DataContext = this;
        }

        public OperationsViewModel ViewModel { get; set; }

        public bool KeepAlive
        {
            get { return false; }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, System.Action<bool> continuationCallback)
        {
            if (ViewModel.ModelChanged)
            {
                var result = MessageBox.Show(
                    Translations.ShouldSaveCaption,
                    Translations.ShouldSave,
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    continuationCallback(false);
                }
                else if (result == MessageBoxResult.No)
                {
                    continuationCallback(true);
                }
                else if (result == MessageBoxResult.Yes)
                {
                    ViewModel.Save.Execute();
                    continuationCallback(true);
                }
            }
            else
            {
                continuationCallback(true);
            }          
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}
