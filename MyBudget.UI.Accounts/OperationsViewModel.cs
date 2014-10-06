using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using MyBudget.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;

namespace MyBudget.UI.Accounts
{
    public class OperationsViewModel : BindableBase
    {
        private IContext _context;
        private IRepository<BankOperation> _operationRepository;

        public OperationsViewModel(IContext context)
        {
            _context = context;
            _operationRepository = context.GetRepository<IRepository<BankOperation>>();
            InitializeFilteringProperties();
            InitializeGrouppingProperties();
            FilterDate = DateTime.Now;
            ResetListData();
            defferedDataUpdate = new DeferredAction(
                Dispatcher.CurrentDispatcher,
                () => { if (Data != null) Data.Refresh(); },
                500);
            Save = new DelegateCommand(DoSave);
        }

        private void InitializeGrouppingProperties()
        {
            string[] groupProperties = new[] 
            { 
                null,
                "BankAccount",
                "Type", 
                "OrderDate", 
                "ExecutionDate", 
                "Title",
                "Category"
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
                "BankAccount", 
                "Type", 
                "OrderDate", 
                "ExecutionDate", 
                "Amount", 
                "Title",
                "Description",
                "Category",
                "SubCategory",
            };
            FilteringProperties = BuildPropertyList(filterProperties);
        }

        private void ResetListData()
        {
            var list = new ListCollectionView(_operationRepository.GetAll().ToList());
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
            return ba.ExecutionDate.Month == FilterDate.Month && ba.ExecutionDate.Year == FilterDate.Year;
        }

        private DateTime _filterDate;
        public DateTime FilterDate
        {
            get
            {
                return _filterDate;
            }
            set
            {
                _filterDate = value;
                OnPropertyChanged(() => FilterDate);
                if (Data != null)
                {
                    Data.Refresh();
                }
            }
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

            return typeof(BankOperation)
                .GetProperty(FilterProperty.Name)
                .GetValue(bo).ToString().ToLowerInvariant()
                .Contains(Filter.ToLowerInvariant());
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
                Data.GroupDescriptions.Add(new PropertyGroupDescription(_groupProperty.Name, new MyBudget.UI.Core.Controls.FixedUiToStringConverter()));
            }
        }

        #endregion

        public DelegateCommand Save { get; set; }
        private void DoSave()
        {            
            _context.SaveChanges();
        }
    }
}
