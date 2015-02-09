using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public class ClassificationRuleXmlRepository : AbstractXmlRepository<ClassificationRule, int>
    {
        public override void Add(ClassificationRule obj)
        {
            obj.Id = LastKey + 1;
            base.Add(obj);
        }
    }
}
