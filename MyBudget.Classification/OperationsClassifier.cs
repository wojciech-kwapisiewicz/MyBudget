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

        public OperationsClassifier(IRepository<ClassificationDefinition> classificationRepository, IRepository<BankAccount> accountsRepository)
        {
            _definitionsToApply = classificationRepository
                .GetAll()
                .Select(def => 
                    new RegularExpressionClassificationRule(def, accountsRepository));
        }

        public IEnumerable<ClassificationResult> ClasifyOpearations(IEnumerable<BankOperation> operations)
        {
            List<ClassificationResult> classificationResult = new List<ClassificationResult>();
            foreach (var currentOperation in operations)
            {
                var foundMatches = _definitionsToApply
                    .Where(r1 => r1.DoMatch(currentOperation))
                    .Select(r2 => new RuleMatch()
                        {
                            MatchedDefinition = r2.Definition,
                            Description = r2.GetCustomDescription()
                        });
                var subResult = new ClassificationResult() { BankOperation = currentOperation, Matches = foundMatches };
                classificationResult.Add(subResult);
            }
            return classificationResult;
        }

        public int ApplyClassificationResult(IEnumerable<ClassificationResult> classificationResult)
        {
            int i = 0;
            foreach (var item in classificationResult)
            {
                if (item.Matches.Count() == 1)
                {
                    item.BankOperation.Category = item.Matches.Single()
                        .MatchedDefinition.Category;
                    item.BankOperation.SubCategory = item.Matches.Single()
                        .MatchedDefinition.SubCategory;
                    i++;
                }
            }
            return i;
        }
    }
}
