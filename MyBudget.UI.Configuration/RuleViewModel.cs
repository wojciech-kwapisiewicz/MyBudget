using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
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

        private IRepository<ClassificationRule, int> _rulesRepository;
        private IContext _context;

        public RuleViewModel(IContext context, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _context = context;
            _rulesRepository = context.GetRepository<IRepository<ClassificationRule, int>>();

            GoBack = new DelegateCommand(DoGoBack);
            Save = new DelegateCommand(DoSave);
        }

        private ClassificationRule _Data;
        public ClassificationRule Data
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
                _rulesRepository.Add(Data);
            }
            _context.SaveChanges();
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(RulesView).FullName);
        }

        public DelegateCommand GoBack { get; set; }
        private void DoGoBack()
        {
            Journal.GoBack();
        }

        public void OnNavigatedTo(ClassificationRule selected, string patternParameter)
        {
            if (selected == null)
            {
                EditMode = EditMode.Add;
                ClassificationRule newRule = new ClassificationRule();
                newRule.FieldName = GetNameFromExpression(a => a.Description);

                if(patternParameter!=null)
                {
                    newRule.Parameter = patternParameter;
                }

                Data = newRule;
            }
            else
            {
                EditMode = EditMode.Edit;
                Data = _rulesRepository.Get(selected.Id);
            }
        }

        private string GetNameFromExpression(Expression<Func<BankOperation, object>> expr)
        {
            MemberExpression body = expr.Body as MemberExpression;
            return body.Member.Name;
        }
    }
}
