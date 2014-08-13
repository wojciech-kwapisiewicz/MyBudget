using Microsoft.Practices.Unity;
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
            unityContainer.RegisterType<IBankAccountRepository, InMemoryBankAccountRepository>(
                new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBankOperationTypeRepository, InMemoryBankOperationTypeRepository>(
                new ContainerControlledLifetimeManager());
        }
    }
}
