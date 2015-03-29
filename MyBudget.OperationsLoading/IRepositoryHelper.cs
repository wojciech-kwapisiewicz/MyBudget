using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading
{
    public interface IRepositoryHelper
    {
        BankAccount GetOrAddAccount(string accountNumber);
        BankOperationType GetOrAddOperationType(string typeName);
    }
}
