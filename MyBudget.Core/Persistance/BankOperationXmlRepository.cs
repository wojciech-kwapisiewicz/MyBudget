using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public class BankOperationXmlRepository : AbstractXmlRepository<BankOperation, int>
    {
        public override void Add(BankOperation obj)
        {
            obj.Id = StoredElemets + 1;
            base.Add(obj);
        }
    }
}
