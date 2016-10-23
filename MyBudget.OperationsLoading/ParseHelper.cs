using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading
{
    public class ParseHelper
    {
        public DateTime ParseDate(string executionDate, string format)
        {
            DateTime parsedExecutionDate = DateTime.ParseExact(
                executionDate,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None);
            return parsedExecutionDate;
        }

        public decimal ParseDecimalInvariant(string amount)
        {
            decimal parsedAmount = decimal.Parse(
                amount,
                NumberStyles.Number,
                CultureInfo.InvariantCulture.NumberFormat);
            return parsedAmount;
        }

        public decimal ParseDecimalPolish(string amount)
        {
            decimal parsedAmount = decimal.Parse(
                amount,
                NumberStyles.Number,
                CultureInfo.GetCultureInfo("pl"));
            return parsedAmount;
        }

        public string GetFirstNCharacters(string description, int n = 30)
        {
            if (description.Length > n)
            {
                return description.Substring(0, n);
            }
            return description;
        }
    }
}
