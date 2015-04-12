using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public class OperationsClassifier : IOperationsClassifier
    {
        private IEnumerable<RegularExpressionClassificationRule> _rulesToApply;

        public OperationsClassifier(IEnumerable<ClassificationRule> rules)
        {
            _rulesToApply = rules.Select(rule => new RegularExpressionClassificationRule(rule));
        }

        public IEnumerable<ClassificationResult> ClasifyOpearations(IEnumerable<BankOperation> operations)
        {
            foreach (var currentOperation in operations)
            {
                var foundMatches = _rulesToApply
                    .Where(r1 => r1.DoMatch(currentOperation))
                    .Select(r2 => new RuleMatch()
                        {
                            Rule = r2.Rule,
                            Description = r2.GetCustomDescription()
                        });

                yield return new ClassificationResult()
                {
                    BankOperation = currentOperation,
                    Matches = foundMatches
                };
            }
        }
    }
}
