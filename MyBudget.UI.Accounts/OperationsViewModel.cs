using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core;
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
    public class CustomFilterProperty<T>
    {
        public string PropertyName{get;set;}
        public Func<T, string, bool> FilteringFunction { get; set; }
    }

    public class OperationsViewModel : BindableBase
    {
        IRepository<BankOperation> _operationRepository;
        LocalizedReflection _reflectionHelper;

        public OperationsViewModel(
            PkoBpParser parser,
            IRepository<BankOperation> operationRepository,
            LocalizedReflection reflectionHelper)
        {
            _reflectionHelper = reflectionHelper;
            _operationRepository = operationRepository;

            FilterDate = DateTime.Now;
            ResetListData();

            defferedDataUpdate = new DeferredAction(
                Dispatcher.CurrentDispatcher,
                () => { if (Data != null) Data.Refresh(); },
                500);
        }

        private void ResetListData()
        {
            var list = new ListCollectionView(_operationRepository.GetAll().ToList());
            list.Filter = a => DatePredicateFilter(a) && FieldPredicateFilter(a);
            Data = list;
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
            if(FilterProperty==null || string.IsNullOrEmpty(Filter))
            {
                return true;
            }

            BankOperation bo = obj as BankOperation;
            var f = CustomFilterProperties.SingleOrDefault(a => a.PropertyName == FilterProperty);
            if(f!=null)
            {
                return f.FilteringFunction(bo, Filter);
            }

            return _reflectionHelper
                .GetPropertyValue<BankOperation>(obj, FilterProperty)
                .ToString().ToLowerInvariant()
                .Contains(Filter.ToLowerInvariant());
        }
        
        DeferredAction defferedDataUpdate;

        public IEnumerable<string> ObjectProperties
        {
            get
            {
                return
                    CustomFilterProperties.Select(a => a.PropertyName).Concat(
                    _reflectionHelper.GetPropertiesNames(typeof(BankOperation)));
            }
        }

        public List<CustomFilterProperty<BankOperation>> CustomFilterProperties = new List<CustomFilterProperty<BankOperation>>
        {
            new CustomFilterProperty<BankOperation>()
            {
                PropertyName = "Konto bankowe",
                FilteringFunction = (operation,filter) => 
                    operation.BankAccount!=null && 
                    operation.BankAccount.Description.ToLowerInvariant()
                        .Contains(filter.ToLowerInvariant())
            }
        };


        private string _filterProperty;
        public string FilterProperty
        {
            get
            {
                return _filterProperty;
            }
            set
            {
                _filterProperty = value;
                OnPropertyChanged(() => FilterProperty);
                Data.Refresh();
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
    }
}
