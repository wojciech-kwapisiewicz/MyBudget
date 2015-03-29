﻿using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.MilleniumAccount
{
    public class MilleniumParser : IParser
    {
        public string Name
        {
            get
            {
                return Resources.MilleniumName;
            }
        }

        public string SupportedFileExtensions
        {
            get
            {

                return Resources.MilleniumFilter;
            }
        }

        ParseHelper _parseHelper;
        IRepositoryHelper _repositoryHelper;

        public MilleniumParser(ParseHelper parseHelper, IRepositoryHelper repositoryHelper)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _parseHelper = parseHelper;
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            _repositoryHelper = repositoryHelper;
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();
                //ignore headers
                List<BankOperation> ops = new List<BankOperation>();
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    ops.Add(ParseLine(line, ++i));
                }
                return ops;
            }
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            string[] lines = inputString.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            List<BankOperation> ops = new List<BankOperation>(lines.Length);
            for (int i = 1; i < lines.Length; i++)
            {
                ops.Add(ParseLine(lines[i], i));
            }

            return ops;
        }

        private BankOperation ParseLine(string line, int lineNumber)
        {
            string[] entries = line.Split(',')
                .Select(a => a.Trim('"')).ToArray();
            var bo = new BankOperation();

            string accountNumber = entries[0]
                .Replace(" ", "")
                .Substring(2, 26);
            string typeName = entries[3];
            string description = entries[6];
            string title = ExtractTitle(description);

            BankAccount account = _repositoryHelper.GetOrAddAccount(accountNumber);

            string amount = entries[7];
            if (string.IsNullOrEmpty(amount))
            {
                amount = entries[8];
            }
            if (string.IsNullOrEmpty(amount))
            {
                throw new FormatException("Invalid format of parsing message");
            }

            return new BankOperation()
            {
                LpOnStatement = lineNumber,
                BankAccount = account,
                OrderDate = _parseHelper.ParseDate(entries[1], "yyyy-MM-dd"),
                ExecutionDate = _parseHelper.ParseDate(entries[2], "yyyy-MM-dd"),
                Amount = _parseHelper.ParseDecimalInvariant(amount),
                EndingBalance = _parseHelper.ParseDecimalInvariant(entries[9]),
                Title = title,
                Description = description,
                Type = _repositoryHelper.GetOrAddOperationType(typeName),
                Cleared = true
            };
        }

        private string ExtractTitle(string description)
        {
            int maxDesc = 20;
            if (description.Length > maxDesc)
            {
                return description.Substring(0, maxDesc);
            }
            return description;
        }
    }
}
