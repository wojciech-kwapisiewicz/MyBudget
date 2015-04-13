using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public class RegularExpressionClassificationRule : IClassificationRule
    {
        public ClassificationDefinition Definition { get; private set; }

        public RegularExpressionClassificationRule(ClassificationDefinition definition)
        {
            Definition = definition;
        }

        public CustomDescription GetCustomDescription()
        {
            return new CustomDescription() { Category = Definition.Category, SubCategory = Definition.SubCategory };
        }

        public bool DoMatch(BankOperation operation)
        {
            return Definition.Rules.Any(a => Matches(a, operation));
        }

        public bool Matches(ClassificationRule rule, BankOperation operation)
        {
            string value = operation.Description;
                //typeof(BankOperation).GetField(ClassificationRule.FieldName).GetValue(operation) as string;
            bool matchFound = Regex.IsMatch(value, rule.RegularExpression, RegexOptions.IgnoreCase);
            return matchFound;
        }
    }
}
