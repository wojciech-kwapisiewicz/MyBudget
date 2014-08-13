using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core;
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
    public partial class OperationsView : UserControl, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        PkoBpParser _parser;

        public OperationsView(
            OperationsViewModel viewModel,
            PkoBpParser parser)
        {
            _parser = parser;
            ViewModel = viewModel;
            LoadFileCommand = new DelegateCommand(() => ViewModel.Data = LoadFromFile());
            SaveCommand = new DelegateCommand(() => Save());
            InitializeComponent();
        }

        public OperationsViewModel ViewModel { get; set; }

        public ICommand LoadFileCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public IEnumerable<BankOperation> LoadFromFile()
        {
            using (Stream stream = new FileDialogService().OpenFile())
            {
                return _parser.Parse(stream);
            }
        }

        public void Save()
        {
            MessageBox.Show("Saved!");
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

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            bool goAhead = MessageBox.Show("Are you sure?", "??", MessageBoxButton.OKCancel) == MessageBoxResult.OK;
            continuationCallback(goAhead);
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
