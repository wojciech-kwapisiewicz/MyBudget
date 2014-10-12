using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MyBudget.Core.ImportData
{
    public class PkoBpParser : IParser
    {
        IParseHelper _parseHelper;

        public PkoBpParser(IParseHelper parseHelper)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _parseHelper = parseHelper;
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            XDocument doc = XDocument.Load(stream);
            return GetEntriesFromXDocument(doc);
        }
        public IEnumerable<BankOperation> Parse(string inputString)
        {
            if(string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentException("No content to parse", inputString);
            }
            XDocument doc = XDocument.Parse(inputString);
            return GetEntriesFromXDocument(doc);
        }

        private IEnumerable<BankOperation> GetEntriesFromXDocument(XDocument document)
        {
            var accountNumber = document.XPathSelectElements("/account-history/search/account").Single().Value;
            BankAccount account = _parseHelper.GetAccount(accountNumber);

            int lp = 0;

            var operations = document.XPathSelectElements("//operations/operation");
            foreach (var operation in operations)
            {
                lp++;
                string orderDate = operation.Descendants("order-date").Single().Value;
                string executionDate = operation.Descendants("exec-date").Single().Value;

                string amount = operation.Descendants("amount").Single().Value;
                string endingBalance = operation.Descendants("ending-balance").Single().Value;
                string description = operation.Descendants("description").Single().Value;

                string typeName = operation.Descendants("type").Single().Value;

                string title = ExtractTitle(description);

                yield return new BankOperation()
                {
                    LpOnStatement = lp,
                    BankAccount = account,
                    OrderDate = _parseHelper.ParseDate(orderDate, "yyyy-MM-dd"),
                    ExecutionDate = _parseHelper.ParseDate(executionDate, "yyyy-MM-dd"),
                    Amount = _parseHelper.ParseDecimalInvariant(amount),
                    EndingBalance = _parseHelper.ParseDecimalInvariant(endingBalance),
                    Title = title,
                    Description = description,
                    Type = _parseHelper.GetOperationType(typeName)
                };
            }
        }

        private string ExtractTitle(string description)
        {
            string titlePrefix = "Tytuł: ";
            int start = description.IndexOf(titlePrefix, StringComparison.InvariantCultureIgnoreCase);
            if (start != -1)
            {
                int end = description.IndexOf('\n', start);
                if (end == -1)
                {
                    return description.Substring(start + titlePrefix.Length);
                }
                else
                {
                    return description.Substring(start + titlePrefix.Length, end - start - titlePrefix.Length);
                }
            }
            return description.IndexOf('\n') == -1 ? 
                description : 
                string.Empty;
        }
    }


}
