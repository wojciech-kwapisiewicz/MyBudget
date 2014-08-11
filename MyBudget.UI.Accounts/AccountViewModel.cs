using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.Model;
using MyBudget.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Accounts
{
    public class AccountViewModel : BindableBase
    {
        public IRegionNavigationJournal Journal { get; set; }
        private IRegionManager _regionManager;

        public AccountViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            GoBack = new DelegateCommand(DoGoBack);
            Save = new DelegateCommand(DoSave);
        }

        private BankAccount _Data;
        public BankAccount Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged(() => Data);
            }
        }

        public DelegateCommand Save { get; set; }
        private void DoSave()
        {
            //_repository.Save();
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(AccountsView).FullName);
        }

        public DelegateCommand GoBack { get; set; }
        private void DoGoBack()
        {
            Journal.GoBack();
        }

    }
}
