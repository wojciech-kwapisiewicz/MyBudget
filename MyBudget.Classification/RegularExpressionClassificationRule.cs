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
        private string _fieldName;
        private string _regularExpression;

        public ClassificationRule Rule { get; private set; }

        public RegularExpressionClassificationRule(ClassificationRule rule)
        {
            _fieldName = rule.FieldName;
            _regularExpression = rule.Parameter;
            Rule = rule;
        }

        public bool DoMatch(BankOperation operation)
        {
            string value = typeof(BankOperation).GetField(_fieldName).GetValue(operation) as string;
            return Regex.IsMatch(value, _regularExpression, RegexOptions.IgnoreCase);
        }

        public CustomDescription GetCustomDescription()
        {
            return new CustomDescription() { Category = Rule.Category, SubCategory = Rule.SubCategory };
        }
    }
}
