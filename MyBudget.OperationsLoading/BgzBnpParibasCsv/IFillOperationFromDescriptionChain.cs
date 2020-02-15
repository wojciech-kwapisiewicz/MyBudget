using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BgzBnpParibasCsv
{
    public interface IFillOperationFromDescriptionChain
    {
        void Match(BankOperation operation, string description);
    }
}
