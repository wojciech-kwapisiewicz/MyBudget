using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.Model;
using MyBudget.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyBudget.UI.Accounts
{
    public class AccountsViewModel : BindableBase
    {
        IRegionManager _regionManager;

        public AccountsViewModel(
            IRegionManager regionManager,
            IBankAccountRepository bankAccountsRepository)
        {
            _regionManager = regionManager;

            AddAccount = new DelegateCommand(NavigateToAdd);
            EditAccount = new DelegateCommand(NavigateToEdit, () => SelectedItem != null);

//            NavigateAccounts = new DelegateCommand(() =>
//    regionManager.RequestNavigate(RegionNames.MainContent, typeof(AccountsView).FullName));

            Data = bankAccountsRepository.GetAll();
            //    new List<BankAccount>()
            //{
            //    new BankAccount(){Id=0, Name="Konto PKO0", Number="0000000", Description="Konto normalne"},
            //    new BankAccount(){Id=1, Name="Konto PKO1", Number="0000001", Description="Konto normalne"},
            //    new BankAccount(){Id=2, Name="Konto PKO2", Number="0000002", Description="Konto normalne"},
            //    new BankAccount(){Id=2, Name="Konto PKO2", Number="0000002"},
            //    new BankAccount(){Id=2, Name="Konto PKO2", Number="0000002", Description="Konto normalne"},
            //    new BankAccount(){Id=2, Name="Konto PKO2", Number="0000002", Description="Konto normalne"}
            //};
        }

        private IEnumerable<BankAccount> _Data;
        public IEnumerable<BankAccount> Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged(() => Data);
            }
        }

        private BankAccount _SelectedItem;
        public BankAccount SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(() => SelectedItem);
                EditAccount.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddAccount { get; set; }
        private void NavigateToAdd()
        {
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(AccountView).FullName);
        }

        public DelegateCommand EditAccount { get; set; }
        private void NavigateToEdit()
        {
            var parameters = new NavigationParameters();
            parameters.Add("selected", SelectedItem);
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(AccountView).FullName, parameters);

        }
        public DelegateCommand DeleteAccount { get; set; }
    }
}
