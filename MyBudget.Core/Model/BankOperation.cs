using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankOperation : IIdentifiable<int>
    {
        public int Id { get; set; }
        public BankAccount BankAccount { get; set; }

        public BankOperationType Type { get; set; }
        public int LpOnStatement { get; set; }        
        public DateTime OrderDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal EndingBalance { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public CustomDescription CustomDescription { get; set; }
    }
}
