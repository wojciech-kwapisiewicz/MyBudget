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

            var groupping = itemsToDisplay.GroupBy(l1 => l1.Category ?? string.Empty);
            var roots = GetGroup(groupping);

            foreach (var item in roots.Where(a => !string.IsNullOrEmpty(a.Key)).OfType <StatisticsGroup>())
            {
                var subGroupping = item.Elements.GroupBy(a => a.SubCategory ?? string.Empty);
                if (subGroupping.Any(a => !string.IsNullOrEmpty(a.Key)))
                {
                    item.SubGroups = GetGroup(subGroupping);
                }
            }

            var overall = roots.OfType<StatisticsGroup>().Sum(a => a.Sum);
            roots.Add(new Splitter() { Key = "==============", Sum = "==============" });
            roots.Add(new StatisticsGroup() { Key = Resources.Translations.SumText, Sum = overall });

            Items = roots;
        }

        private ObservableCollection<IGroupItem> GetGroup(IEnumerable<IGrouping<string, BankOperation>> groupping)
        {
            return new ObservableCollection<IGroupItem>(groupping.Select(group =>
                    new StatisticsGroup()
                    {
                        Key = group.Key,
                        Elements = group,
                        Sum = group.Sum(el1 => el1.Amount)
                    }));
        }

        private IEnumerable<IGroupItem> _items { get; set; }
        public IEnumerable<IGroupItem> Items
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
