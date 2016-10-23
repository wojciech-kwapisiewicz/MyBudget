using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.Model;

namespace MyBudget.OperationsLoading.BgzBnpParibas
{
    public class BgzBnpParibasParser : IParser
    {
        public string Name
        {
            get
            {
                return Resources.BgzBnpParibasName;
            }
        }

        public string SupportedFileExtensions
        {
            get
            {
                return string.Format("{0} {1}", Resources.BgzBnpParibasName, Resources.CsvFilter);
            }
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
