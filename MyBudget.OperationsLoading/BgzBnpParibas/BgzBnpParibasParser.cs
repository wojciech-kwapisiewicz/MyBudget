using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBudget.Model;
using LumenWorks.Framework.IO.Csv;
using System.Text.RegularExpressions;

namespace MyBudget.OperationsLoading.BgzBnpParibas
{
    public class BgzBnpParibasParser : IParser
    {        
        private const string RegexTransakcjaKarta = @"OPERACJA KARTĄ .* ([0-9]{6}X{6}[0-9]{4}) [0-9]{6} TRAN SAKCJA BEZGOTOWKOWA (.*) ([1-9][0-9]*.[0-9]{2}[A-Z]{3}) D=([0-9]{2}.[0-9]{2}.[0-9]{4}).*";
        private const string RegexPrzelew = @"PRZELEW NA RACHUNEK NUMER ([0-9]{2} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}) (.*)";
        private const string RegexPrzelewPrzychodzacy = @"PRZELEW UZNANIOWY \(NADANO ([0-9]{2}-[0-9]{2}-[0-9]{4})\) (.*)";
        private const string RegexPrzelewDoInnegoBanku = @"PRZELEW OBCIĄŻENIOWY (.*) ([0-9]{2} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}) (.*)";
            
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

        private ParseHelper _parseHelper;
        private IFillOperationFromDescriptionChain _fillDescription;
        private IRepositoryHelper _repositoryHelper;

        public BgzBnpParibasParser(ParseHelper parseHelper, IRepositoryHelper repositoryHelper, IFillOperationFromDescriptionChain fillDescription)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            if (fillDescription == null)
                throw new ArgumentNullException("fillDescription");
            _parseHelper = parseHelper;
            _repositoryHelper = repositoryHelper;
            _fillDescription = fillDescription;
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            List<BankOperation> operations = new List<BankOperation>();
            using (var reader = new CsvReader(new StreamReader(stream), true, ';'))
            {
                while (reader.ReadNextRecord())
                {
                    BankOperation operation = new BankOperation();
                    operation.BankAccount = _repositoryHelper.GetOrAddAccount("BGZBNPParibas");
                    operation.ExecutionDate = _parseHelper.ParseDate(reader[0], "yyyy-MM-dd");
                    operation.OrderDate = operation.ExecutionDate;
                    operation.Amount = _parseHelper.ParseDecimalInvariant(reader[1]);
                    operation.EndingBalance = _parseHelper.ParseDecimalInvariant(reader[3]);
                    _fillDescription.Match(operation, reader[2]);
                    operations.Add(operation);
                }
            }

            return operations;
        }
    }
}
