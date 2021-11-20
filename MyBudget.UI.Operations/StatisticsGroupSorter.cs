using System.Collections.Generic;
using System.Linq;

namespace MyBudget.UI.Operations
{
    public class StatisticsGroupSorter : IComparer<StatisticsGroup>
    {
        private Dictionary<string, int> ItemsOrder = new Dictionary<string, int>
        {
            { "Dochody",1 },
            { "Opłaty",2 },
            { "Jedzenie",3 },
            { "Samochód",4 },
            { "Zakupy",5 }
        };

        public int Compare(StatisticsGroup x, StatisticsGroup y)
        {
            if (ItemsOrder.Keys.Contains(x.Key) && ItemsOrder.Keys.Contains(y.Key))
            {
                return ItemsOrder[x.Key] - ItemsOrder[y.Key];
            }

            if (ItemsOrder.Keys.Contains(x.Key) && !ItemsOrder.Keys.Contains(y.Key))
            {
                return -1;
            }

            if (!ItemsOrder.Keys.Contains(x.Key) && ItemsOrder.Keys.Contains(y.Key))
            {
                return 1;
            }

            return x.Key.CompareTo(y.Key);
        }
    }
}
