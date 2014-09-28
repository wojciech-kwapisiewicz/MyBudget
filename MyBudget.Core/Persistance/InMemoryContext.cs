using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public class InMemoryContext : IContext
    {
        Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();
        Dictionary<Type, IRepository> _extendedRepositories = new Dictionary<Type, IRepository>();

        public T GetRepository<T>() where T : IRepository
        {
            IRepository retVal;

            Type repositoryType = typeof(T);
            if (!_repositories.TryGetValue(repositoryType, out retVal))
            {
                _extendedRepositories.TryGetValue(repositoryType, out retVal);
            }

            return (T)retVal;
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
