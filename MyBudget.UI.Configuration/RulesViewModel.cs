﻿using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.UI.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            EditRule = new DelegateCommand(NavigateToEdit, () => SelectedDefinitions.Count() == 1);
            DeleteRule = new DelegateCommand(GoDelete, () => SelectedDefinitions.Count() == 1);
            MergeRules = new DelegateCommand(DoMergeRules, () => SelectedDefinitions.Count() > 1);
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

        private IEnumerable _SelectedItems;
        public IEnumerable SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
                OnPropertyChanged(() => SelectedItems);
                OnPropertyChanged(() => SelectedDefinitions);
                EditRule.RaiseCanExecuteChanged();
                DeleteRule.RaiseCanExecuteChanged();
                MergeRules.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<ClassificationDefinition> SelectedDefinitions
        {
            get
            {
                if(SelectedItems==null)
                {
                    return Enumerable.Empty<ClassificationDefinition>();
                }
                return SelectedItems.Cast<ClassificationDefinition>();
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
            parameters.Add("selected", SelectedDefinitions.Single());
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(RuleView).FullName, parameters);

        }

        public DelegateCommand DeleteRule { get; set; }
        private void GoDelete()
        {
            _definitionsRepository.Delete(SelectedDefinitions.Single());
            _context.SaveChanges();
            OnPropertyChanged(() => Data);
        }

        public DelegateCommand MergeRules { get; set; }
        private void DoMergeRules()
        {
            var noCat = SelectedDefinitions.Select(a => a.Category).Distinct().Count();
            var noSubCat = SelectedDefinitions.Select(a => a.SubCategory).Distinct().Count();
            var first = SelectedDefinitions.First();
            if (noCat != 1 || noSubCat != 1)
            {
                MessageBoxResult continueResult = MessageBox.Show(
                    string.Format(
                        Resources.Translations.MergeRulesConflictMsg, 
                        Environment.NewLine, first.Description, 
                        first.Category, 
                        first.SubCategory),
                    Resources.Translations.MergeRulesConflictCaption,
                    MessageBoxButton.OKCancel);
                if (continueResult == MessageBoxResult.OK)
                {
                    InternalMerge(first, SelectedDefinitions);
                }
            }
            else
            {
                InternalMerge(first, SelectedDefinitions);
            }
        }

        private void InternalMerge(ClassificationDefinition first, IEnumerable<ClassificationDefinition> selectedDefinitions)
        {
            foreach (var item in selectedDefinitions)
            {
                if (item != first)
                {
                    foreach (var rule in item.Rules)
                    {
                        first.Rules.Add(rule);
                    }
                    _definitionsRepository.Delete(item);
                }
            }
            _context.SaveChanges();
            OnPropertyChanged(() => Data);
        }
    }
}
