using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.PkoBpCreditCard
{
    public class CreditCardPageDefinition
    {
        private int StartPageTextLength = 16;
        public const string StartPageRegex = @"rozliczenia\(PLN\)";
        public const string EndPageRegex = @"strona [0-9]/ [0-9]";
        public const string SummaryText = "Saldo początkowe: Dokonane operacje: Wpłaty: Odsetki: Całkowite zadłużenie:";

        public int BeginIndex { get; set; }
        public int EndIndex { get; set; }

        public string GetPageContent(string input)
        {
            int begin = BeginIndex + StartPageTextLength;
            int end = EndIndex;
            int beginOfSummary = input.IndexOf(SummaryText, begin);
            if (beginOfSummary > begin && beginOfSummary < end)
            {
                end = beginOfSummary;
            }

            string page = input.Substring(begin, end - begin).Trim();
            return page;
        }
    }
}
