using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core
{
    public interface IClassifier
    {
        CustomDescription GetCustomDescription(BankOperation entry);
    }
}
