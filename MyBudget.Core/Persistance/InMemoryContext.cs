using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Persistance
{
    public class InMemoryContext : IContext
    {
        private IRepositoryFactory _repositoryFactory;

        public InMemoryContext(IRepositoryFactory repositoryFactory)
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
