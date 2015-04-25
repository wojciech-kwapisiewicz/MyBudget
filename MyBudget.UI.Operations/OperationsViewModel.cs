using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using MyBudget.Classification;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.UI.Configuration;
using MyBudget.UI.Core;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MyBudget.UI.Operations
{
    public class OperationsViewModel : BindableBase
    {
        private IContext _context;
        private IRepository<BankOperation> _operationRepository;

        private IResolveClassificationConflicts _resolveConflicts;
        private IRegionManager _regionManager;

        public OperationsViewModel(IContext context, IRegionManager regionManager, IResolveClassificationConflicts resolveConflicts)
        {
            _context = context;
            _operationRepository = context.GetRepository<IRepository<BankOperation>>();

            _resolveConflicts = resolveConflicts;
            _regionManager = regionManager;            

            InitializeFilteringProperties();
            InitializeGrouppingProperties();
                        
            defferedDataUpdate = new DeferredAction(
                Dispatcher.CurrentDispatcher,
                () => { if (Data != null) Data.Refresh(); },
                500);

            ApplyRules = new DelegateCommand(() => DoApplyRules());
            ClearRules = new DelegateCommand(() => DoClearRules());
            Save = new DelegateCommand(DoSave);
            SelectNext = new DelegateCommand<bool?>(DoSelectNext);
            CreateRule = new DelegateCommand(DoCreateRule, () => SelectedOperation != null);
        }

        private Func<DateTime, bool> _filterFunction;
        public Func<DateTime, bool> FilterFunction
        {
            get
            {
                return _filterFunction;
            }
            set
            {
                _filterFunction = value;                
                if (Data != null)
                {
                    Data.Refresh();
                }
                else
                {
                    ResetListData();
                }
            }
        }

        public bool ModelHasChanged
        {
            get
            {
                return _context.DataHasChanged();
            }

        }
        private void InitializeGrouppingProperties()
        {
            string[] groupProperties = new[] 
            { 
                null,
                "OrderDate", 
                "BankAccount",
                "Category",
                "Type", 
                "Title",
                "ExecutionDate", 
            };
            GrouppingProperties = BuildPropertyList(groupProperties);
        }

        private static List<PropertyDescription> BuildPropertyList(string[] groupProperties)
        {
            List<PropertyDescription> _groupProperties = new List<PropertyDescription>();
            foreach (var item in groupProperties)
            {
                if (item == null)
                {
                    _groupProperties.Add(new PropertyDescription());
                }
                else
                {
                    _groupProperties.Add(new PropertyDescription()
                    {
                        Name = item,
                        Translation = ResourceManagerHelper.GetTranslation(typeof(BankOperation), item)
                    });
                }
            }
            return _groupProperties;
        }

        private void InitializeFilteringProperties()
        {
            string[] filterProperties = new[] 
            { 
                "Title",
                "Description",
                "Category",
                "SubCategory",
                "Amount", 
                //"Type", 
                "BankAccount", 
                "OrderDate", 
                "ExecutionDate", 
            };
            FilteringProperties = BuildPropertyList(filterProperties);
        }

        public void ResetListData()
        {
            var list = new ListCollectionView(_operationRepository.GetAll()
                .OrderBy(a => a.OrderDate)
                .ThenBy(b => b.ExecutionDate)
                .ToList());
            list.Filter = a => DatePredicateFilter(a) && FieldPredicateFilter(a);
            Data = list;
        }

        private BankOperation _SelectedOperation;
        public BankOperation SelectedOperation
        {
            get
            {
                return _SelectedOperation;
            }
            set
            {
                _SelectedOperation = value;
                OnPropertyChanged(() => SelectedOperation);
                OnPropertyChanged(() => Categories);
                OnPropertyChanged(() => SubCategories);
                CreateRule.RaiseCanExecuteChanged();
            }
        }

        private ListCollectionView _data;
        public ListCollectionView Data
        {
            get
            {

                return _data; 
            }
            set
            {
                _data = value;
                OnPropertyChanged(() => Data);
            }
        }


        public Type GridType { get { return typeof(BankOperation); } }

        #region Filtering

        bool DatePredicateFilter(object obj)
        {            
            BankOperation ba = obj as BankOperation;
            return FilterFunction(ba.OrderDate);
        }

        bool FieldPredicateFilter(object obj)
        {
            if (FilterProperty == null || string.IsNullOrEmpty(Filter))
            {
                return true;
            }

            BankOperation bo = obj as BankOperation;
            var f = CustomFilterProperties.SingleOrDefault(a => a.PropertyName == FilterProperty.Name);
            if (f != null)
            {
                return f.FilteringFunction(bo, Filter);
            }

            var value = typeof(BankOperation)
                .GetProperty(FilterProperty.Name)
                .GetValue(bo);

            if (value == null)
            {
                return false;
            }
            else
            {
                return value.ToString().ToLowerInvariant()
                .Contains(Filter.ToLowerInvariant());
            }
        }
        
        DeferredAction defferedDataUpdate;

        public IEnumerable<PropertyDescription> FilteringProperties
        {
            get;
            private set;
        }

        public List<CustomFilterProperty<BankOperation>> CustomFilterProperties = new List<CustomFilterProperty<BankOperation>>
        {
            new CustomFilterProperty<BankOperation>()
            {
                PropertyName = "BankAccount",
                FilteringFunction = (operation,filter) => 
                    operation.BankAccount!=null && 
                    operation.BankAccount.Description.ToLowerInvariant()
                        .Contains(filter.ToLowerInvariant())
            }
        };

        private PropertyDescription _filterProperty;
        public PropertyDescription FilterProperty
        {
            get
            {
                return _filterProperty;
            }
            set
            {
                _filterProperty = value;
                OnPropertyChanged(() => FilterProperty);
                if (Data != null)
                {
                    Data.Refresh();
                }
            }
        }

        private string _filter;
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnPropertyChanged(() => Filter);
                defferedDataUpdate.Go();
            }
        }

        #endregion

        #region Groupping

        public IEnumerable<PropertyDescription> GrouppingProperties
        {
            get; private set;
        }

        private PropertyDescription _groupProperty;
        public PropertyDescription GroupProperty
        {
            get
            {
                return _groupProperty;
            }
            set
            {
                _groupProperty = value;
                OnPropertyChanged(() => GroupProperty);
                UpdateGroupProperty();
            }
        }

        private void UpdateGroupProperty()
        {
            Data.GroupDescriptions.Clear();
            if (_groupProperty != null && _groupProperty.Name != null)
            {
                Data.GroupDescriptions.Add(new PropertyGroupDescription(_groupProperty.Name, new MyBudget.UI.Core.Converters.FixedUiToStringConverter()));
            }
        }

        #endregion

        public IEnumerable<string> Categories
        {
            get
            {
                if (SelectedOperation == null)
                    return Enumerable.Empty<string>();
                return SortByCount(_operationRepository.GetAll().Select(a => a.Category));
            }
        }

        public IEnumerable<string> SortByCount(IEnumerable<string> source)
        {
            return source.GroupBy(a => a)
                    .OrderBy(b => b.Count())
                    .Select(c => c.Key);
        }

        public IEnumerable<string> SubCategories
        {
            get
            {
                if (SelectedOperation == null)
                    return Enumerable.Empty<string>();
                return SortByCount(_operationRepository.GetAll()
                    .Where(a => a.Category == SelectedOperation.Category).Select(b => b.SubCategory));
            }
        }

        public DelegateCommand Save { get; set; }
        private void DoSave()
        {            
            _context.SaveChanges();
        }

        public DelegateCommand<bool?> SelectNext { get; set; }
        private void DoSelectNext(bool? directionForward)
        {
            if (!directionForward.HasValue) return;
            bool moved = directionForward.Value ? Data.MoveCurrentToNext() : Data.MoveCurrentToPrevious();
            if (moved && OperationHasBeenSelected != null && Data.CurrentItem is BankOperation)
            {
                OperationHasBeenSelected((BankOperation)Data.CurrentItem);
            }
        }

        public DelegateCommand ApplyRules { get; set; }
        private void DoApplyRules()
        {
            var classifier = new OperationsClassifier(
                _context.GetRepository<IRepository<ClassificationDefinition>>(),
                _context.GetRepository<IRepository<BankAccount>>());
            var classificationResult = classifier.ClasifyOpearations(Data.OfType<BankOperation>());

            _resolveConflicts.ResolveConflicts(classificationResult);
            var assigned = classifier.ApplyClassificationResult(classificationResult);
            var unassigned = classificationResult.Count() - assigned;

            MessageBox.Show(string.Format(
                "Dla {0} operacji przypisano kategorię.{1}Dla {2} operacji nie przypisano kategorii.",
                assigned, Environment.NewLine, unassigned));
        }
        
        public DelegateCommand ClearRules { get; set; }
        private void DoClearRules()
        {
            if (MessageBoxResult.Yes == MessageBox.Show(
                "Na pewno usunąć kategorię i podkategorię we wszystkich operacjach?",
                "Potwierdź",
                MessageBoxButton.YesNo))
            {
                foreach (var item in Data.OfType<BankOperation>())
                {
                    item.Category = null;
                    item.SubCategory = null;
                }
            }
        }

        public Action<BankOperation> OperationHasBeenSelected { private get; set; }

        public DelegateCommand CreateRule { get; set; }       

        private void DoCreateRule()
        {
            var parameters = new NavigationParameters();
            parameters.Add("template", SelectedOperation);
            _regionManager.RequestNavigate(RegionNames.MainContent, typeof(RuleView).FullName, parameters);
        }        
    }
}
