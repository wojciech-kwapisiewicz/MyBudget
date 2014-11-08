using java.io;
using MyBudget.Core.Model;
using MyBudget.Core.Pdf;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public class PkoBpCreditClearedParser : IParser
    {
        public string Name
        {
            get
            {
                return Resources.PkoBpCardClearedName;
            }
        }

        public string SupportedFileExtensions
        {
            get
            {
                return Resources.PkoBpCardClearedFilter;
            }
        }

        private IParseHelper _parseHelper;
        private PdfReader _pdfReader;
        private CreditCardTextParsing _ccTextParsing;
        private CreditCardClearedTextParsing _clearedTextParsing;
        private CreditCardPagesExtractor _pagesExtractor;

        public PkoBpCreditClearedParser(
            IParseHelper parseHelper,
            PdfReader pdfReader,
            CreditCardPagesExtractor pagesExtractor,
            CreditCardTextParsing ccTextParsing,
            CreditCardClearedTextParsing clearedTextParsing)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            if (pdfReader == null)
                throw new ArgumentNullException("pdfReader");
            if (pagesExtractor == null)
                throw new ArgumentNullException("pagesExtractor");
            if (ccTextParsing == null)
                throw new ArgumentNullException("ccTextParsing");
            if (clearedTextParsing == null)
                throw new ArgumentNullException("clearedTextParsing");

            _parseHelper = parseHelper;
            _pdfReader = pdfReader;
            _pagesExtractor = pagesExtractor;
            _ccTextParsing = ccTextParsing;
            _clearedTextParsing = clearedTextParsing;
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            string text = _pdfReader.GetStringFromPdfStream(stream);

            return Parse(text);
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            string cardNumber = _ccTextParsing.ExtractCardNumber(
                inputString,
                CreditCardTextParsing.CardNumberStartStringCleared);
            BankAccount account = _parseHelper.GetAccount(cardNumber);
            var pages = _pagesExtractor.GetPages(inputString);
            string[] ops = pages
                .SelectMany(x => _clearedTextParsing.ExtractOperationsLines(x))
                .ToArray();

            for (int i = 0; i < ops.Length; i++)
            {
                CreditCardOperationDetails details = _clearedTextParsing.ParseDetails(ops[i]);
                string operationType = 
                    details.Description.Contains(CreditCardTextParsing.RepaymentOperationName) ?
                    CreditCardTextParsing.RepaymentType : CreditCardTextParsing.StandardType;

                yield return new BankOperation()
                {
                    LpOnStatement = i + 1,
                    BankAccount = account,
                    OrderDate = _parseHelper.ParseDate(details.OrderDate, CreditCardTextParsing.DateFormat),
                    ExecutionDate = _parseHelper.ParseDate(details.ExecutionDate, CreditCardTextParsing.DateFormat),
                    Amount = -_parseHelper.ParseDecimalPolish(details.Amount),
                    Title = _ccTextParsing.ExtractTitle(details.Description),
                    Description = details.Description,
                    Type = _parseHelper.GetOperationType(operationType)
                };
            }
        }
    }
}
