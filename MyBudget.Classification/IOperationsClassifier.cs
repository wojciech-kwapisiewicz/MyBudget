using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Classification
{
    public interface IOperationsClassifier
    {
        IEnumerable<ClassificationResult> ClasifyOpearations(IEnumerable<BankOperation> operations);

        int ApplyClassificationResult(IEnumerable<ClassificationResult> classificationResult);
    }
}
