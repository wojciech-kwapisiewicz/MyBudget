using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading
{
    public interface IParser
    {
        string Name { get; }
        string SupportedFileExtensions { get; }

        IEnumerable<BankOperation> Parse(Stream stream);
        IEnumerable<BankOperation> Parse(string inputString);
    }
}
