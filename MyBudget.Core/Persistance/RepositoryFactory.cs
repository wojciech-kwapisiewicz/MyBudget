using MyBudget.Core.DataContext;
using MyBudget.Core.InMemoryPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Persistance
{
    public class RepositoryFactory
    {
        private Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();
        public Dictionary<Type, IRepository> Repositories { get
            {
                return _repositories;
            }
        }
        
        private Dictionary<Type, IRepository> _extendedRepositories = new Dictionary<Type, IRepository>();
        public Dictionary<Type, IRepository> ExtendedRepositories
        {
            get
            {
                return _extendedRepositories;
            }
        }

        public RepositoryFactory(
            BankAccountXmlRepository ba,
            BankOperationTypeXmlRepository bot,
            BankStatementXmlRepository bs,
            BankOperationXmlRepository bo
            )
        {
            foreach (var repo in new IRepository[] { ba, bot, bs, bo })
            {
                RegisterRepository(repo);
            }              
        }

        private void RegisterRepository(IRepository accountsRepository)
        {
            Type repositoryType = accountsRepository.GetType();
            _repositories.Add(repositoryType, accountsRepository);
            foreach (var repositoryInterface in
                repositoryType.GetInterfaces().Where(a => 
                    a!= typeof(IRepository) && typeof(IRepository).IsAssignableFrom(a)))
            {
                _extendedRepositories.Add(repositoryInterface, accountsRepository);
            }
        }

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
    }
}
