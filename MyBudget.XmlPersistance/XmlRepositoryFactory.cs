using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.XmlPersistance
{
    public class XmlRepositoryFactory
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

        public XmlRepositoryFactory(
            BankAccountXmlRepository ba,
            CardXmlRepository ca,
            BankOperationTypeXmlRepository bot,
            BankStatementXmlRepository bs,
            BankOperationXmlRepository bo,
            ClassificationDefinitionXmlRepository cr
            )
        {
            foreach (var repo in new IRepository[] { ba, ca, bot, bs, bo, cr })
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
