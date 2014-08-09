using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core
{
    public interface IClassifier
    {
        CustomDescription GetCustomDescription(BankAccountEntry entry);
    }
}
