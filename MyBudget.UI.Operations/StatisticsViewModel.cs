using Microsoft.Practices.Prism.Mvvm;
using MyBudget.Core.DataContext;
using MyBudget.Model;
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
            ReloadView();
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
                OnPropertyChanged(() => FilterFunction);
                ReloadView();
            }
        }

        private bool? _cleared;
        public bool? Cleared
        {
            get
            {
                return _cleared;
            }
            set
            {
                _cleared = value;
                OnPropertyChanged(() => Cleared);
                ReloadView();
            }
        }        

        private void ReloadView()
        {
            var itemsToDisplay = _bankOperationRepository.GetAll();
            if(Cleared.HasValue)
            {
                itemsToDisplay = itemsToDisplay.Where(a => a.Cleared == Cleared.Value);
            }
            if(FilterFunction!=null)
            {
                itemsToDisplay = itemsToDisplay.Where(a => FilterFunction(a.OrderDate));
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

            var sum1 = roots.OfType<StatisticsGroup>().Sum(a => a.Sum);
            var sum2 = roots.OfType<StatisticsGroup>().Where(b => b.Key != "Wewnetrzne").Sum(a => a.Sum);
            roots.Add(new Splitter() { Key = "==============", Sum = "==============" });
            roots.Add(new StatisticsGroup() 
            { 
                Key = Resources.Translations.SumText, 
                Sum = sum1
            });
            roots.Add(new StatisticsGroup()
            {
                Key = "Bez wewnetrznych",
                Sum = sum2
            });

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
    }
}
