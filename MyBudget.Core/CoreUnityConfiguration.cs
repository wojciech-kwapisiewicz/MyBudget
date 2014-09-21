using Microsoft.Practices.Unity;
using MyBudget.Core.DataContext;
using MyBudget.Core.InMemoryPersistance;
using MyBudget.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core
{
    public class CoreUnityConfiguration
    {
        public void Configure(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<BankAccountInMemoryRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankOperationInMemoryRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankOperationTypeInMemoryRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankStatementInMemoryRepository>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IContext, Context>();

            unityContainer.RegisterType<IRepository<BankAccount>, BankAccountInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankAccount, string>, BankAccountInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankOperation>, BankOperationInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankOperation, int>, BankOperationInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankOperationType>, BankOperationTypeInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankOperationType, string>, BankOperationTypeInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankStatement>, BankStatementInMemoryRepository>();
            unityContainer.RegisterType<IRepository<BankStatement, int>, BankStatementInMemoryRepository>();
        }
    }
}
