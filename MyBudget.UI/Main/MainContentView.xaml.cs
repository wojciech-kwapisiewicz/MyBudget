using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core;
using MyBudget.UI.Services;
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

namespace MyBudget.UI.Main
{
    /// <summary>
    /// Interaction logic for MainContentView.xaml
    /// </summary>
    public partial class MainContentView : UserControl, INavigationAware
    {
        public MainContentView(BankEntriesViewModel viewModel)
        {
            ViewModel = viewModel;
            LoadFile = new DelegateCommand(() => ViewModel.Data = Load());
            InitializeComponent();
        }

        public BankEntriesViewModel ViewModel { get; set; }

        public ICommand LoadFile { get; set; }

        public IEnumerable<BankAccountEntry> Load()
        {
            using (Stream stream = new FileDialogService().OpenFile())
            {
                return new PkoBpParser().Parse(stream);
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


    public class BankEntriesViewModel : BindableBase
    {


        private IEnumerable<BankAccountEntry> _Data;
        public IEnumerable<BankAccountEntry> Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged(() => Data);
            }
        }
    }
}
