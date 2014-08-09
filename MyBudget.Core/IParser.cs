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
        IEnumerable<BankAccountEntry> Parse(Stream stream);
        IEnumerable<BankAccountEntry> Parse(string inputString);
    }
}
