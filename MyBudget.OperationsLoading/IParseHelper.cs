using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading
{
    public interface IParseHelper
    {
        BankAccount GetAccount(string accountNumber);
        BankOperationType GetOperationType(string typeName);
        DateTime ParseDate(string executionDate, string format);
        decimal ParseDecimalInvariant(string amount);
        decimal ParseDecimalPolish(string amount);
    }
}
