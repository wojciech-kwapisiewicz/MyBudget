using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
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
        IRepository<BankAccount> _bankAccountsRepository;

        public AccountsViewModel(
            IRegionManager regionManager,
            IRepository<BankAccount> bankAccountsRepository)
        {
            _regionManager = regionManager;
            _bankAccountsRepository = bankAccountsRepository;

            AddAccount = new DelegateCommand(NavigateToAdd);
            EditAccount = new DelegateCommand(NavigateToEdit, () => SelectedItem != null);
            DeleteAccount = new DelegateCommand(GoDeleteAccount, () => SelectedItem != null);
        }

        public IEnumerable<BankAccount> Data
        {
            get { return _bankAccountsRepository.GetAll(); }
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
                DeleteAccount.RaiseCanExecuteChanged();
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
        private void GoDeleteAccount()
        {
            _bankAccountsRepository.Delete(SelectedItem);
            OnPropertyChanged(() => Data);
        }
    }
}
