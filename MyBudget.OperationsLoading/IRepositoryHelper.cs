using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MyBudget.OperationsLoading
{
    public interface IRepositoryHelper
    {
        BankAccount GetOrAddAccount(string accountNumber);
        BankOperationType GetOrAddOperationType(string typeName);
        Card GetOrAddCard(string cardNumber);
    }
}
