using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.UI.Core;
using MyBudget.UI.Core.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyBudget.UI.Configuration
{    
    public class RuleViewModel : BindableBase
    {
        public EditMode EditMode { get; set; }

        public IRegionNavigationJournal Journal { get; set; }
        private IRegionManager _regionManager;

        private IRepository<ClassificationDefinition, int> _definitionsRepository;
        private IContext _context;
        private BankAccount[] _existingAccounts;

        public RuleViewModel(IContext context, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _context = context;
            _definitionsRepository = context.GetRepository<IRepository<ClassificationDefinition, int>>();
            _existingAccounts = context.GetRepository<IRepository<BankAccount>>().GetAll().ToArray();
            GoBack = new DelegateCommand(DoGoBack);
            Save = new DelegateCommand(DoSave);
            DeleteRule = new DelegateCommand<object>(DoDeleteRule);
        }

        private ClassificationDefinition _Data;
        public ClassificationDefinition Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged(() => Data);
                Rules = new ObservableCollection<WrappedClassificationRule>(
                    Data.Rules.Select(a => new WrappedClassificationRule(a, _existingAccounts)));
            }
        }

        private ObservableCollection<WrappedClassificationRule> _Rules;
        public ObservableCollection<WrappedClassificationRule> Rules
        {
            get { return _Rules; }
            set
            {
                _Rules = value;
                OnPropertyChanged(() => Rules);
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

        public DelegateCommand<object> DeleteRule { get; set; }
        private void DoDeleteRule(object parameter)
        {          
            var ruleToRemove = Rules.First(a => a.Data == parameter);
            Data.Rules.Remove(ruleToRemove.Data);
            Rules.Remove(ruleToRemove);
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
