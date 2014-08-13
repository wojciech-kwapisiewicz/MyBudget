using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core
{
    public interface IParser
    {
        IEnumerable<BankOperation> Parse(Stream stream);
        IEnumerable<BankOperation> Parse(string inputString);
    }
}
