using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MyBudget.Core
{
    public class PkoBpParser : IParser
    {
        public IEnumerable<BankAccountEntry> Parse(Stream stream)
        {
            XDocument doc = XDocument.Load(stream);
            return GetEntriesFromXDocument(doc);
        }
        public IEnumerable<BankAccountEntry> Parse(string inputString)
        {
            XDocument doc = XDocument.Parse(inputString);
            return GetEntriesFromXDocument(doc);
        }

        private IEnumerable<BankAccountEntry> GetEntriesFromXDocument(XDocument document)
        {
            var operations = document.XPathSelectElements("//operations/operation");
            foreach (var operation in operations)
            {
                string amount = operation.Descendants("amount").Single().Value;
                string description = operation.Descendants("description").Single().Value;
                string executionDate = operation.Descendants("exec-date").Single().Value;
                DateTime parsedExecutionDate = DateTime.ParseExact(
                    executionDate,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                decimal parsedAmount = decimal.Parse(
                    amount,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture.NumberFormat);

                yield return new BankAccountEntry()
                {
                    Amount = parsedAmount,
                    ExecutionDate = parsedExecutionDate,
                    Description = description
                };
            }
        }
    }
}
