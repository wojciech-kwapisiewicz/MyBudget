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
        public string Name
        {
            get
            {
                return Resources.PkoBpName;
            }
        }

        public string SupportedFileExtensions
        {
            get
            {
                return Resources.PkoBpFilter;
            }
        }
        
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
                    Type = _parseHelper.GetOperationType(typeName),
                    Cleared = true
                };
            }
        }

        private string ExtractTitle(string description)
        {
            string[] lines = description.Split(
                new string[] { Environment.NewLine, "\n", "\r\n" },
                StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();

            string titleLinePrefix = "Tytuł: ";
            string titleLine = lines.FirstOrDefault(a =>
                a.StartsWith(titleLinePrefix, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrWhiteSpace(titleLine))
            {
                return description;
            }
            else
            {
                titleLine = titleLine.Replace(titleLinePrefix, "");
            }

            string title = string.Empty;

            string refNumber = string.Empty;
            string refLinePrefix = "Numer referencyjny: ";
            string refLine = lines.FirstOrDefault(a => 
                a.StartsWith(refLinePrefix, StringComparison.OrdinalIgnoreCase));

            string locationPrefix = "Lokalizacja: ";
            string locationLine = lines.FirstOrDefault(a =>
                a.StartsWith(locationPrefix, StringComparison.OrdinalIgnoreCase));


            if (!string.IsNullOrWhiteSpace(refLine) &&
                !string.IsNullOrWhiteSpace(locationPrefix) &&
                titleLine.Contains(refLine.Replace(refLinePrefix, "")))
            {
                return ReorderLocationLine(locationLine);
            }

            return titleLine;
        }

        private string ReorderLocationLine(string line)
        {          
            string addressPrefix = "Adres: ";
            string countryPrefix = "Kraj: ";
            string cityPrefix = "Miasto: ";

            string regex = string.Format(
                "{0}(.*){1}(.*){2}(.*)",
                countryPrefix,
                cityPrefix,
                addressPrefix);

            var match = Regex.Match(line, regex);

            if (match.Success && match.Groups.Count == 4)
            {
                return string.Format("{0} {1}{2}{3}{4}", 
                    match.Groups[3],
                    cityPrefix, 
                    match.Groups[2], 
                    countryPrefix, 
                    match.Groups[1]).TrimEnd();
            }

            return line;
        }
    }
}
