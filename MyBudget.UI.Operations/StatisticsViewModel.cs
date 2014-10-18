using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Operations
{
    public class StatisticsGroup
    {
        public string Key { get; set; }
        public decimal Sum { get; set; }
        public IEnumerable<BankOperation> Elements { get; set; }
        public IEnumerable<StatisticsGroup> SubGroups { get; set; }
    }

    public class StatisticsViewModel : BindableBase
    {
        IRepository<BankOperation> _bankOperationRepository;

        public StatisticsViewModel(IContext context)
        {
            _bankOperationRepository = context.GetRepository<IRepository<BankOperation>>();
            FilterMonth = DateTime.Now.Date;
            StartDate = DateTime.Now.Date.AddMonths(-1);
            EndDate = DateTime.Now.Date.AddMonths(1);
            ReloadView();
        }

        private void ReloadView()
        {
            var itemsToDisplay = _bankOperationRepository.GetAll();

            if(FilterByMonth)
            {
                itemsToDisplay = itemsToDisplay.Where(a => 
                    a.OrderDate.Month == FilterMonth.Month && 
                    a.OrderDate.Year == FilterMonth.Year);
            }
            else
            {
                itemsToDisplay = itemsToDisplay.Where(a =>
                    a.OrderDate.Date >= StartDate.Date &&
                    a.OrderDate.Date <= EndDate.Date);
            }

            var its = GetGroup(itemsToDisplay.GroupBy(l1 => l1.Category));

            foreach (var item in its.Where(a => a.Key != null))
            {
                item.SubGroups = GetGroup(item.Elements.GroupBy(a => a.SubCategory));
            }

            Items = its;
        }

        private IEnumerable<StatisticsGroup> GetGroup(IEnumerable<IGrouping<string, BankOperation>> groupping)
        {
            return new ObservableCollection<StatisticsGroup>(groupping.Select(group =>
                    new StatisticsGroup()
                    {
                        Key = group.Key,
                        Elements = group,
                        Sum = group.Sum(el1 => el1.Amount)
                    }));
        }

        private IEnumerable<StatisticsGroup> _items { get; set; }
        public IEnumerable<StatisticsGroup> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged(() => Items);
            }
        }

        private bool _filterByMonth;
        public bool FilterByMonth
        {
            get
            {
                return _filterByMonth;
            }
            set
            {
                _filterByMonth = value;
                OnPropertyChanged(() => FilterByMonth);
                OnPropertyChanged(() => FilterByRange);
                ReloadView();
            }
        }

        private DateTime _filterMonth;
        public DateTime FilterMonth
        {
            get
            {
                return _filterMonth;
            }
            set
            {
                _filterMonth = value;
                OnPropertyChanged(() => FilterMonth);
                ReloadView();
            }
        }

        public bool FilterByRange
        {
            get
            {
                return !_filterByMonth;
            }
            set
            {
                FilterByMonth = !value;                
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged(() => StartDate);
                ReloadView();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged(() => EndDate);
                ReloadView();
            }
        }
    }
}
