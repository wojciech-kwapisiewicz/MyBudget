using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public class PkoBpCreditCardParser : IParser
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

        const string StandardType = "Karta kredytowa";
        const string RepaymentType = "Spłata karty";
        const string RepaymentOperationName = "SPŁATA NALEŻNOŚCI - DZIĘKUJEMY";

        const string Start = "Data operacji	Data księgowania	Opis	Kwota operacji	Kwota w PLN";
        const string EndForCurrent = "Suma operacji rozliczonych";
        const string EndForHistory = "Przewodnik Demo Bezpieczeństwo Regulaminy Opłaty Oprocentowanie Kursy walut Gwarantowanie depozytów Kod BIC (Swift)";

        IParseHelper _parseHelper;

        public PkoBpCreditCardParser(IParseHelper parseHelper)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _parseHelper = parseHelper;
        }


        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return Parse(reader.ReadToEnd());
            }
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            string body = ExtractBody(inputString);
            string cardNumber = ExtractCardNumber(body);

            BankAccount account = _parseHelper.GetAccount(cardNumber);

            string[] ops = ExtractOperationsLines(ref body);

            for (int i = 0; i < ops.Length; i++)
            {
                string[] details = ops[i].Split('\t');
                string operationType = details[2].Contains(RepaymentOperationName) ?
                    RepaymentType : StandardType;

                yield return new BankOperation()
                {
                    LpOnStatement = i + 1,
                    BankAccount = account,
                    OrderDate = _parseHelper.ParseDate(details[0], "yyyy-MM-dd"),
                    ExecutionDate = _parseHelper.ParseDate(details[1], "yyyy-MM-dd"),
                    Amount = -_parseHelper.ParseDecimalPolish(details[4]),
                    Title = ExtractTitle(details[2]),
                    Description = details[2],
                    Type = _parseHelper.GetOperationType(operationType)
                };
            }
        }

        private static string ExtractBody(string inputString)
        {
            
            int startIndex = inputString.IndexOf(Start);
            int endIndex = inputString.IndexOf(EndForCurrent);
            if (endIndex < 0)
            {
                endIndex = inputString.IndexOf(EndForHistory);
            }
            string body = inputString.Substring(startIndex, endIndex - startIndex);
            return body;
        }
        
        private string ExtractCardNumber(string body)
        {
            string cardNumberStartString = "Informacje podstawowe\r\nKarta";
            var indexOfStartCard = body.IndexOf(cardNumberStartString);
            string cardNumber = string.Empty;
            if (indexOfStartCard < 0)
            {
                throw new InvalidOperationException(
                    "Loading of cleared operations is not supported. Pick different parser.");
                //cardNumber = "CRED";
            }
            else
            {
                indexOfStartCard += cardNumberStartString.Length;
                int x = body.IndexOf("\r\n", indexOfStartCard);
                cardNumber = body.Substring(indexOfStartCard, x - indexOfStartCard);
            }

            return cardNumber.Trim().PadLeft(26, '*');
        }

        private string[] ExtractOperationsLines(ref string body)
        {
            body = RemoveLine(body, 1);
            string splitter = "Drukuj";
            string[] ops = body
                .Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries)
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(b => b.Trim()).ToArray();
            return ops;
        }

        public string RemoveLine(string input)
        {
            int newLine = input.IndexOf("\r\n");
            return input.Substring(newLine + 2);
        }

        public string RemoveLine(string input, int no)
        {
            for (int i = 0; i < no; i++)
            {
                input = RemoveLine(input);
            }
            return input;
        }

        private string ExtractTitle(string description)
        {
            int maxDesc = 20;
            int newLine = description.IndexOf('\r');
            int toExtract = maxDesc;
            if(newLine>=0 && newLine < maxDesc)
            {
                toExtract = newLine;
            }

            if (description.Length > toExtract)
            {
                return description.Substring(0, toExtract);
            }
            return description;
        }
    }
}
