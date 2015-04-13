using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.XmlPersistance
{
    public class ClassificationDefinitionXmlRepository : AbstractXmlRepository<ClassificationDefinition, int>
    {
        public override void Add(ClassificationDefinition obj)
        {
            obj.Id = LastKey + 1;
            base.Add(obj);
        }
    }
}
