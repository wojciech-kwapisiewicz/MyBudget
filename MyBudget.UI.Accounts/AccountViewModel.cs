using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using MyBudget.UI.Core;

namespace MyBudget.UI.Accounts
{
    public class AccountViewModel : BindableBase
    {
        public EditMode EditMode { get; set; }

        public IRegionNavigationJournal Journal { get; set; }
        private IRegionManager _regionManager;

        private IRepository<BankAccount, string> _bankAccountRepository;
        private IContext _context;

        public AccountViewModel(IContext context,IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _context = context;
            _bankAccountRepository = context.GetRepository<IRepository<BankAccount, string>>();
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
            if (EditMode == EditMode.Add)
            {
                _bankAccountRepository.Add(Data);
            }
            _context.SaveChanges();
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(AccountsView).FullName);
        }

        public DelegateCommand GoBack { get; set; }
        private void DoGoBack()
        {
            Journal.GoBack();
        }

        public void OnNavigatedTo(BankAccount selected)
        {
            if (selected == null)
            {
                Data = new BankAccount();
                EditMode = EditMode.Add;
            }
            else
            {
                Data = _bankAccountRepository.Get(selected.Id);
                EditMode = EditMode.Edit;
            }
        }
    }
}
