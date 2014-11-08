using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public class CreditCardOperationDetails
    {
        public string OrderDate { get; set; }
        public string ExecutionDate { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
    }
}
