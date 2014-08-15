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
    public class OperationsViewModel : BindableBase
    {
        IRepository<BankOperation> _operationRepository;

        public OperationsViewModel(
            PkoBpParser parser,
            IRepository<BankOperation> operationRepository)
        {
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
            return obj.GetPropertyValue<BankOperation>(FilterProperty).ToString().Contains(Filter);
        }
        
        DeferredAction defferedDataUpdate;

        public IEnumerable<string> ObjectProperties
        {
            get
            {
                return typeof(BankOperation).GetPropertiesNames();
            }
        }

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
