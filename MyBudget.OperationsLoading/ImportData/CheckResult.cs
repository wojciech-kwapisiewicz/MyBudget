using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.ImportData
{
    public class CheckResult
    {
        public BankOperation ExistingOperation { get; set; }
        public CheckStatus Status { get; set; }
    }
}
