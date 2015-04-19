using MyBudget.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Services
{
    public interface IResolveClassificationConflicts
    {
        void ResolveConflicts(IEnumerable<ClassificationResult> classificationResult);
    }
}
