using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBudget.Core.Model
{
    public interface IBankAccountRepository
    {
        BankAccount Get(string accountNumber);
        IEnumerable<BankAccount> GetAll();
        void Add(BankAccount bankAccount);
    }
}
