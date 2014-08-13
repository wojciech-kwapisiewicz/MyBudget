using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBudget.Core.Model
{
    public interface IBankOperationTypeRepository
    {
        BankOperationType Get(string name);
        void Add(BankOperationType bankOperationType);
    }
}
