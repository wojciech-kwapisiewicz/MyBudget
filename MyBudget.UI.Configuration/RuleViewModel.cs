using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Configuration
{    
    public class RuleViewModel : BindableBase
    {
        public EditMode EditMode { get; set; }

        public IRegionNavigationJournal Journal { get; set; }
        private IRegionManager _regionManager;

        private IRepository<ClassificationDefinition, int> _definitionsRepository;
        private IContext _context;

        public RuleViewModel(IContext context, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _context = context;
            _definitionsRepository = context.GetRepository<IRepository<ClassificationDefinition, int>>();

            GoBack = new DelegateCommand(DoGoBack);
            Save = new DelegateCommand(DoSave);
        }

        private ClassificationDefinition _Data;
        public ClassificationDefinition Data
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
                _definitionsRepository.Add(Data);
            }
            _context.SaveChanges();
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(RulesView).FullName);
        }

        public DelegateCommand GoBack { get; set; }
        private void DoGoBack()
        {
            Journal.GoBack();
        }

        public void OnNavigatedTo(ClassificationDefinition selected, BankOperation templateOperation)
        {
            if (selected == null)
            {
                EditMode = EditMode.Add;
                ClassificationDefinition newDefinition = new ClassificationDefinition();
                ClassificationRule newRule = new ClassificationRule();
                newDefinition.Rules.Add(newRule);

                if (templateOperation != null)
                {
                    newDefinition.Category = templateOperation.Category;
                    newDefinition.SubCategory = templateOperation.SubCategory;                    
                    newRule.RegularExpression = templateOperation.Description;
                    newRule.Account = templateOperation.BankAccount.Number;
                    newRule.CounterAccount = templateOperation.CounterAccount;
                }

                Data = newDefinition;
            }
            else
            {
                EditMode = EditMode.Edit;
                Data = _definitionsRepository.Get(selected.Id);
            }
        }

        private string GetNameFromExpression(Expression<Func<BankOperation, object>> expr)
        {
            MemberExpression body = expr.Body as MemberExpression;
            return body.Member.Name;
        }
    }
}
