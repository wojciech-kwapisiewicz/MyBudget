using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Classification
{
    public class RegularExpressionClassifier : IClassifier
    {
        private string _fieldName;
        private string _regularExpression;

        public RegularExpressionClassifier(string fieldName, string regularExpression)
        {
            _fieldName = fieldName;
            _regularExpression = regularExpression;
        }

        public CustomDescription GetCustomDescription(BankOperation entry)
        {
            string value = typeof(BankOperation).GetField(_fieldName).GetValue(entry) as string;
            return null;
        }
    }
}
