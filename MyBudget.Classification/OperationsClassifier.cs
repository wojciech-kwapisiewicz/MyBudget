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
        private IEnumerable<RegularExpressionClassificationRule> _definitionsToApply;

        public OperationsClassifier(IEnumerable<ClassificationDefinition> classificationDefinitions)
        {
            _definitionsToApply = classificationDefinitions.Select(def => new RegularExpressionClassificationRule(def));
        }

        public IEnumerable<ClassificationResult> ClasifyOpearations(IEnumerable<BankOperation> operations)
        {
            foreach (var currentOperation in operations)
            {
                var foundMatches = _definitionsToApply
                    .Where(r1 => r1.DoMatch(currentOperation))
                    .Select(r2 => new RuleMatch()
                        {
                            MatchedDefinition = r2.Definition,
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
