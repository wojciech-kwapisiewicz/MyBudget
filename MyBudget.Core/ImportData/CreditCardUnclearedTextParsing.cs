using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public class CreditCardUnclearedTextParsing
    {
        private const string Start = "Data operacji	Data księgowania	Opis	Kwota operacji	Kwota w PLN";
        private const string EndForCurrent = "Suma operacji rozliczonych";
        private const string EndForHistory = "Przewodnik Demo Bezpieczeństwo Regulaminy Opłaty Oprocentowanie Kursy walut Gwarantowanie depozytów Kod BIC (Swift)";
        private const string OperationSplitter = "Drukuj";

        public string ExtractBody(string inputString)
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

        public string[] ExtractOperationsLines(string body)
        {
            string[] ops = body.RemoveLine()
                .Split(new[] { OperationSplitter }, StringSplitOptions.RemoveEmptyEntries)
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(b => b.Trim()).ToArray();
            return ops;
        }
    }

}
