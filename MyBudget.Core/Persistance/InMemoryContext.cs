using MyBudget.Core.DataContext;
using MyBudget.Core.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public class InMemoryContext : IContext
    {
        private RepositoryFactory _repositoryFactory;

        public InMemoryContext(RepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
        }

        public T GetRepository<T>() where T : IRepository
        {
            return _repositoryFactory.GetRepository<T>();
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
