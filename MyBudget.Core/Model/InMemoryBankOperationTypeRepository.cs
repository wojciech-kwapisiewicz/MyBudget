using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class InMemoryBankOperationTypeRepository : IBankOperationTypeRepository
    {
        Dictionary<string, BankOperationType> types = new Dictionary<string, BankOperationType>();

        public BankOperationType Get(string name)
        {
            BankOperationType type;
            types.TryGetValue(name, out type);
            return type;
        }

        public void Add(BankOperationType bankOperationType)
        {
            types.Add(bankOperationType.Name, bankOperationType);
        }
    }
}
