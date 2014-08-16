using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public class BankStatementInMemoryRepository : AbstractInMemoryRepository<BankStatement, int>
    {
        public override void Add(BankStatement obj)
        {
            obj.Id = StoredElemets + 1;
            base.Add(obj);
        }
    }
}
