using Microsoft.Practices.Prism.Commands;
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

namespace MyBudget.UI.Configuration
{
    public class RulesViewModel : BindableBase
    {
        IRegionManager _regionManager;
        IRepository<ClassificationRule, int> _rulesRepository;
        IContext _context;

        public RulesViewModel(IContext context, IRegionManager regionManager)
        {
            _context = context;
            _regionManager = regionManager;
            _rulesRepository = context.GetRepository<IRepository<ClassificationRule, int>>();

            AddRule = new DelegateCommand(NavigateToAdd);
            EditRule = new DelegateCommand(NavigateToEdit, () => SelectedItem != null);
            DeleteRule = new DelegateCommand(GoDelete, () => SelectedItem != null);
        }

        public IEnumerable<ClassificationRule> Data
        {
            get { return _rulesRepository.GetAll(); }
        }

        private ClassificationRule _SelectedItem;
        public ClassificationRule SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(() => SelectedItem);
                EditRule.RaiseCanExecuteChanged();
                DeleteRule.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddRule { get; set; }
        private void NavigateToAdd()
        {
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(RuleView).FullName);
        }

        public DelegateCommand EditRule { get; set; }
        private void NavigateToEdit()
        {
            var parameters = new NavigationParameters();
            parameters.Add("selected", SelectedItem);
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(RuleView).FullName, parameters);

        }

        public DelegateCommand DeleteRule { get; set; }
        private void GoDelete()
        {
            _rulesRepository.Delete(SelectedItem);
            _context.SaveChanges();
            OnPropertyChanged(() => Data);
        }
    }
}
