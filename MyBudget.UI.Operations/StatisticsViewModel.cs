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

        private bool _showSeparate = true;
        public bool ShowSeparate
        {
            get
            {
                return _showSeparate;
            }
            set
            {
                _showSeparate = value;
                OnPropertyChanged(() => ShowSeparate);
                ReloadView();
            }
        }

        private string _SeparateCategories = "Wewnetrzne, Oszczędności";
        public string SeparateCategories
        {
            get
            {
                return _SeparateCategories;
            }
            set
            {
                _SeparateCategories = value;
                OnPropertyChanged(() => SeparateCategories);
                ReloadView();
            }
        }

        private void ReloadView()
        {
            var itemsToDisplay = GetItemsToDisplay();

            var roots = GetGroup(itemsToDisplay.GroupBy(l1 => l1.Category ?? string.Empty));

            foreach (var item in roots.Where(a => !string.IsNullOrEmpty(a.Key)).OfType<StatisticsGroup>())
            {
                var subGroupping = item.Elements.GroupBy(a => a.SubCategory ?? string.Empty)
                    .Where(a => !string.IsNullOrEmpty(a.Key));
                item.SubGroups = GetGroup(subGroupping);
            }

            AddSummary(roots);

            AddSeparateSummaries(roots);

            Items = roots;
        }

        private void AddSeparateSummaries(ObservableCollection<IGroupItem> roots)
        {
            if (ShowSeparate)
            {
                var separateSumaries = SeparateCategories.Split(',')
                    .Select(a => a.Trim());

                var sumWithoutSeparate = roots.OfType<StatisticsGroup>()
                    .Where(b => !separateSumaries.Contains(b.Key))
                    .Sum(a => a.Sum);

                roots.Add(new StatisticsGroup()
                {
                    Key = Resources.Translations.WithoutSparateCategoriesText,
                    Sum = sumWithoutSeparate
                });

                roots.Add(new Splitter() { Key = "==============", Sum = "==============" });
                foreach (var summary in separateSumaries)
                {
                    var summarySum = roots.OfType<StatisticsGroup>()
                        .Where(b => summary == b.Key)
                        .Sum(a => a.Sum);
                    roots.Add(new StatisticsGroup()
                    {
                        Key = summary,
                        Sum = summarySum
                    });
                }
            }
        }

        private static void AddSummary(ObservableCollection<IGroupItem> roots)
        {
            var sumAll = roots.OfType<StatisticsGroup>().Sum(a => a.Sum);
            roots.Add(new Splitter() { Key = "==============", Sum = "==============" });
            roots.Add(new StatisticsGroup()
            {
                Key = Resources.Translations.SumText,
                Sum = sumAll
            });
        }

        private IEnumerable<BankOperation> GetItemsToDisplay()
        {
            var itemsToDisplay = _bankOperationRepository.GetAll();
            if (Cleared.HasValue)
            {
                itemsToDisplay = itemsToDisplay.Where(a => a.Cleared == Cleared.Value);
            }
            if (FilterFunction != null)
            {
                itemsToDisplay = itemsToDisplay.Where(a => FilterFunction(a.OrderDate));
            }
            return itemsToDisplay;
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
