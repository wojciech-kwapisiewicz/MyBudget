using MyBudget.Core.DataContext;
using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankStatement : IIdentifiable<int>
    {
        [DontDisplay]
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime LoadTime { get; set; }
        
        [DontDisplay]
        public IEnumerable<BankOperation> Operations { get; set; }
    }
}
