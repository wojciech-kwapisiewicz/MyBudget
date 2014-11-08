using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public class CreditCardClearedTextParsing
    {
        public List<string> ExtractOperationsLines(string body)
        {
            string beginOperationRegex = string.Format("{0} {0} ", CreditCardTextParsing.DateFormatRegex);

            MatchCollection matches = Regex.Matches(body, beginOperationRegex);

            List<string> operations = new List<string>();

            int endIndex = body.Length;

            var indexes = matches
                .OfType<Match>()
                .Select(a => a.Index)
                .OrderByDescending(b => b);

            List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();
            foreach (var index in indexes)
            {
                ranges.Add(new Tuple<int, int>(index, endIndex));
                endIndex = index;
            }
            ranges.Reverse();

            foreach (var range in ranges)
            {
                operations.Add(
                    body.Substring(range.Item1, range.Item2 - range.Item1));
            }


            return operations;
        }

        public CreditCardOperationDetails ParseDetails(string detailsText)
        {
            MatchCollection datesMatches = Regex.Matches(detailsText, CreditCardTextParsing.DateFormatRegex);
            MatchCollection amountMatch = Regex.Matches(detailsText, CreditCardTextParsing.AmountFormatRegex);

            CreditCardOperationDetails operationDetails = new CreditCardOperationDetails();
            operationDetails.OrderDate = datesMatches[0].Value;
            operationDetails.ExecutionDate = datesMatches[1].Value;
            operationDetails.Amount = amountMatch[amountMatch.Count - 1].Value;

            operationDetails.Description = detailsText
                .ReplaceFirst(operationDetails.OrderDate, string.Empty)
                .ReplaceFirst(operationDetails.ExecutionDate, string.Empty)
                .ReplaceLast(operationDetails.Amount, string.Empty)
                .Trim();

            return operationDetails;
        }
    }
}
