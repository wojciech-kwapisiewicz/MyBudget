using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Operations
{
    public class StatisticsGroup : IGroupItem
    {
        public string Key { get; set; }
        public decimal Sum { get; set; }
        public IEnumerable<BankOperation> Elements { get; set; }
        public IEnumerable<IGroupItem> SubGroups { get; set; }
    }
}
