using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core
{
    public class BankAccountEntry
    {
        public int Id { get; set; }

        public string OperationName { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public DateTime ExecutionDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public CustomDescription CustomDescription { get; set; }
    }
}
