using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{    
    public class CreditCardTextParsing
    {
        public const string StandardType = "Karta kredytowa";
        public const string RepaymentType = "Spłata karty";
        public const string RepaymentOperationName = "SPŁATA NALEŻNOŚCI - DZIĘKUJEMY";
        public const string DateFormat = "dd.MM.yyyy";
        public const string DateFormatRegex = "[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}";
        public const string AmountFormatRegex = "[+-]?[0-9]{1,3}(?: ?[0-9]{3})*\\,[0-9]{2}";
        public const string CardNumberStartStringCleared = "Numer karty: ";
        public const string CardNumberStartStringUncleared = "Informacje podstawowe\r\nKarta";
        public const string CreditCardAccountPrefix = "CreditCard";

        public string ExtractTitle(string description)
        {
            int maxDesc = 20;
            int newLine = description.IndexOf('\r');
            int toExtract = maxDesc;
            if (newLine >= 0 && newLine < maxDesc)
            {
                toExtract = newLine;
            }

            if (description.Length > toExtract)
            {
                return description.Substring(0, toExtract);
            }
            return description;
        }

        public string ExtractCardNumber(string fullText, string cardText)
        {
            int indexOfStartCard = fullText.IndexOf(cardText);
            string cardNumber = string.Empty;
            if (indexOfStartCard < 0)
            {
                throw new InvalidOperationException(Resources.NoCreditCardFound);
            }
            else
            {
                indexOfStartCard += cardText.Length;
                int endOfLine = fullText.IndexOf("\r\n", indexOfStartCard);
                cardNumber = fullText.Substring(
                    indexOfStartCard, 
                    endOfLine - indexOfStartCard);
            }

            return CreditCardAccountPrefix + cardNumber.Trim().PadLeft(16, '*');
        }
    }
}
