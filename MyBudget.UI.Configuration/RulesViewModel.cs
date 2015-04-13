﻿using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
using MyBudget.Model;
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
        IRepository<ClassificationDefinition, int> _definitionsRepository;
        IContext _context;

        public RulesViewModel(IContext context, IRegionManager regionManager)
        {
            _context = context;
            _regionManager = regionManager;
            _definitionsRepository = context.GetRepository<IRepository<ClassificationDefinition, int>>();

            AddRule = new DelegateCommand(NavigateToAdd);
            EditRule = new DelegateCommand(NavigateToEdit, () => SelectedItem != null);
            DeleteRule = new DelegateCommand(GoDelete, () => SelectedItem != null);
        }

        public IEnumerable<ClassificationDefinition> Data
        {
            get
            {
                return _definitionsRepository.GetAll()
                    .OrderBy(a => a.Category)
                    .ThenBy(b => b.SubCategory);
            }
        }

        private ClassificationDefinition _SelectedItem;
        public ClassificationDefinition SelectedItem
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
            _definitionsRepository.Delete(SelectedItem);
            _context.SaveChanges();
            OnPropertyChanged(() => Data);
        }
    }
}
