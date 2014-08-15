using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public class Context : IContext
    {
        public bool SaveChanges()
        {
            return true;
        }
    }
}
