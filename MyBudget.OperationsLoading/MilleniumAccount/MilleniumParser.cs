using LumenWorks.Framework.IO.Csv;
using MyBudget.Model;
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
            List<BankOperation> operations = new List<BankOperation>();
            int line = 1;
            using (var reader = new CsvReader(new StreamReader(stream, Encoding.UTF8), true, ','))
            {
                while (reader.ReadNextRecord())
                {
                    BankOperation operation = new BankOperation() { Cleared = true, LpOnStatement = line++ };

                    string accountNumber = reader[0].Replace(" ", "").Substring(2, 26);
                    operation.BankAccount = _repositoryHelper.GetOrAddAccount(accountNumber);

                    operation.OrderDate = _parseHelper.ParseDate(reader[1], "yyyy-MM-dd");
                    operation.ExecutionDate = _parseHelper.ParseDate(reader[2], "yyyy-MM-dd");
                    operation.Type = _repositoryHelper.GetOrAddOperationType(reader[3]);
                    operation.CounterAccount = reader[4].Replace(" ", "");
                    operation.CounterParty = reader[5];
                    operation.Description = reader[6];
                    operation.Title = _parseHelper.GetFirstNCharacters(operation.Description, OperationsLoadingConsts.OperationTitleLength);

                    string amount = reader[7];
                    if (string.IsNullOrEmpty(amount))
                    {
                        amount = reader[8];
                    }
                    if (string.IsNullOrEmpty(amount))
                    {
                        throw new FormatException("Invalid format of parsing message");
                    }
                    operation.Amount = _parseHelper.ParseDecimalInvariant(amount);
                    operation.EndingBalance = _parseHelper.ParseDecimalInvariant(reader[9]);

                    operations.Add(operation);
                }
            }

            return operations;
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            return Parse(ToStream(inputString));
        }

        public Stream ToStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
