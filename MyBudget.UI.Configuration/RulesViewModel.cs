using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Configuration
{
    public class RulesViewModel
    {
        IRepository<ClassificationRule, int> rulesRepository;

        public RulesViewModel(IContext context)
        {
            rulesRepository = context.GetRepository<IRepository<ClassificationRule, int>>();
        }
    }
}
