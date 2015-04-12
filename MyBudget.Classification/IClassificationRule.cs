using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public interface IClassificationRule
    {
        bool DoMatch(BankOperation operation);

        CustomDescription GetCustomDescription();
    }
}
