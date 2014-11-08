using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public class CreditCardPagesExtractor
    {
        public IEnumerable<string> GetPages(string input)
        {
            List<CreditCardPageDefinition> pageDefinitions = GetPagesDefinitions(input);
            List<string> pages = new List<string>();
            foreach (var pageDefinition in pageDefinitions)
            {
                pages.Add(pageDefinition.GetPageContent(input));
            }
            return pages;
        }

        private static List<CreditCardPageDefinition> GetPagesDefinitions(string input)
        {
            MatchCollection beginings = Regex.Matches(input, CreditCardPageDefinition.StartPageRegex, RegexOptions.Singleline);
            MatchCollection endings = Regex.Matches(input, CreditCardPageDefinition.EndPageRegex, RegexOptions.Singleline);
            List<CreditCardPageDefinition> pages = new List<CreditCardPageDefinition>();
            for (int i = 0; i < beginings.Count; i++)
            {
                pages.Add(new CreditCardPageDefinition()
                {
                    BeginIndex = beginings[i].Index,
                    EndIndex = endings[i].Index
                });
            }
            return pages;
        }
    }

}
