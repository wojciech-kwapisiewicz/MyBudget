using Microsoft.Practices.Prism.Regions;
using MyBudget.UI.Core.Popups;
using MyBudget.UI.Operations.Resources;
using System;
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
        private IMessageBoxService _messageBoxService;

        public OperationsView(IMessageBoxService messageBoxService, OperationsViewModel viewModel)
        {
            _messageBoxService = messageBoxService;
            ViewModel = viewModel;
            viewModel.OperationHasBeenSelected = MoveFocusToCategoryTextBox;
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void MoveFocusToCategoryTextBox(MyBudget.Model.BankOperation selectedOperation)
        {
            //Grab focus on proper insertion textBox
            ((TextBox)OperationCategoryElement.Template.FindName("Text", OperationCategoryElement)).SelectAll();
            //Scroll grid to current item
            operationsGrid.ScrollIntoView(selectedOperation);
        }

        public OperationsViewModel ViewModel { get; set; }

        public bool KeepAlive
        {
            get { return false; }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> navigationCallback)
        {
            if (ViewModel.ModelHasChanged)
            {
                _messageBoxService.ShowMessageBox(
                    Translations.ShouldSaveCaption,
                    Translations.ShouldSaveText,
                    MessageBoxButton.YesNoCancel,
                    (r) => ContinueNavigation(r, navigationCallback));
            }
            else
            {
                navigationCallback(true);
            }
        }

        private void ContinueNavigation(MessageBoxResult result, Action<bool> navigationCallback)
        {
            if (result == MessageBoxResult.Cancel)
            {
                navigationCallback(false);
            }
            else if (result == MessageBoxResult.No)
            {
                navigationCallback(true);
            }
            else if (result == MessageBoxResult.Yes)
            {
                ViewModel.Save.Execute();
                navigationCallback(true);
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
