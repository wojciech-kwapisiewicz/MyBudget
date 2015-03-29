using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.PkoBpCreditCard
{
    public class PkoBpCreditCardUnclearedParser : IParser
    {
        public string Name
        {
            get
            {
                return Resources.PkoBpCardName;
            }
        }

        public string SupportedFileExtensions
        {
            get
            {
                return Resources.PkoBpCardFilter;
            }
        }

        private ParseHelper _parseHelper;
        private IRepositoryHelper _repositoryHelper;
        private CreditCardTextParsing _ccTextParsing;
        private CreditCardUnclearedTextParsing _unclearedParsing;

        public PkoBpCreditCardUnclearedParser(
            ParseHelper parseHelper,
            IRepositoryHelper repositoryHelper,
            CreditCardTextParsing ccTextParsing,
            CreditCardUnclearedTextParsing unclearedParsing)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            if (ccTextParsing == null)
                throw new ArgumentNullException("ccTextParsing");
            if (unclearedParsing == null)
                throw new ArgumentNullException("unclearedParsing");

            _parseHelper = parseHelper;
            _repositoryHelper = repositoryHelper;
            _ccTextParsing = ccTextParsing;
            _unclearedParsing = unclearedParsing;
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return Parse(reader.ReadToEnd());
            }
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            string body = _unclearedParsing.ExtractBody(inputString);
            string cardNumber = _ccTextParsing.ExtractCardNumber(
                inputString,
                CreditCardTextParsing.CardNumberStartStringUncleared);

            BankAccount account = _repositoryHelper.GetOrAddAccount(cardNumber);

            string[] ops = _unclearedParsing.ExtractOperationsLines(body);

            for (int i = 0; i < ops.Length; i++)
            {
                string[] details = ops[i].Split('\t');
                string operationType = 
                    details[2].Contains(CreditCardTextParsing.RepaymentOperationName) ?
                    CreditCardTextParsing.RepaymentType : CreditCardTextParsing.StandardType;

                yield return new BankOperation()
                {
                    LpOnStatement = i + 1,
                    BankAccount = account,
                    OrderDate = _parseHelper.ParseDate(details[0], "yyyy-MM-dd"),
                    ExecutionDate = _parseHelper.ParseDate(details[1], "yyyy-MM-dd"),
                    Amount = -_parseHelper.ParseDecimalPolish(details[4]),
                    Title = _ccTextParsing.ExtractTitle(details[2]),
                    Description = details[2],
                    Type = _repositoryHelper.GetOrAddOperationType(operationType)
                };
            }
        }
    }
}
