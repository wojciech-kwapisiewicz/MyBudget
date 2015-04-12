using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public class ClassificationResult
    {
        public BankOperation BankOperation { get; set; }
        public IEnumerable<RuleMatch> Matches { get; set; }
    }
}
