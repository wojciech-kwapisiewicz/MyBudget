using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class InMemoryBankAccountRepository : IBankAccountRepository
    {
        Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();
        
        public BankAccount Get(string accountNumber)
        {
            BankAccount account;
            accounts.TryGetValue(accountNumber, out account);
            return account;
        }

        public void Add(BankAccount bankAccount)
        {
            accounts.Add(bankAccount.Number, bankAccount);
        }


        public IEnumerable<BankAccount> GetAll()
        {
            return accounts.Values;
        }
    }
}
