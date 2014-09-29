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

namespace MyBudget.Core
{
    public class PkoBpParser : IParser
    {
        private IRepository<BankAccount,string> _bankAccountRepository;
        private IRepository<BankOperationType,string> _bankOperationTypeRepository;

        public PkoBpParser(
            IRepository<BankAccount, string> bankAccountRepository,
            IRepository<BankOperationType, string> bankOperationTypeRepository)
        {
            if (bankAccountRepository == null)
                throw new ArgumentNullException("bankAccountRepository");
            if (bankOperationTypeRepository == null)
                throw new ArgumentNullException("bankOperationTypeRepository");
            _bankAccountRepository = bankAccountRepository;
            _bankOperationTypeRepository = bankOperationTypeRepository;
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
            BankAccount account = GetAccount(document);

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
                    OrderDate = ParseDate(orderDate),
                    ExecutionDate = ParseDate(executionDate),
                    Amount = ParseDecimal(amount),
                    EndingBalance = ParseDecimal(endingBalance),
                    Title = title,
                    Description = description,
                    Type = GetType(typeName)
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

        private BankAccount GetAccount(XDocument document)
        {
            var accountNumber = document.XPathSelectElements("/account-history/search/account").Single().Value;

            BankAccount account = _bankAccountRepository.Get(accountNumber);
            if (account == null)
            {
                account = new BankAccount();
                account.Number = accountNumber;
                _bankAccountRepository.Add(account);
            }
            return account;
        }

        private BankOperationType GetType(string typeName)
        {
            BankOperationType operationType = _bankOperationTypeRepository.Get(typeName);
            if (operationType == null)
            {
                operationType = new BankOperationType();
                operationType.Name = typeName;
                _bankOperationTypeRepository.Add(operationType);
            }
            return operationType;
        }

        private static decimal ParseDecimal(string amount)
        {
            decimal parsedAmount = decimal.Parse(
                amount,
                NumberStyles.Number,
                CultureInfo.InvariantCulture.NumberFormat);
            return parsedAmount;
        }

        private static DateTime ParseDate(string executionDate)
        {
            DateTime parsedExecutionDate = DateTime.ParseExact(
                executionDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None);
            return parsedExecutionDate;
        }
    }
}
