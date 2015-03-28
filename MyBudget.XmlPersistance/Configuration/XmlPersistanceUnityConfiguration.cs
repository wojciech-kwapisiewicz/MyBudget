using Microsoft.Practices.Unity;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.XmlPersistance.Configuration
{
    public class XmlPersistanceUnityConfiguration
    {
        public void Configure(IUnityContainer unityContainer)
        {
            //Repos
            unityContainer.RegisterType<BankAccountXmlRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankOperationXmlRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankOperationTypeXmlRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankStatementXmlRepository>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IRepository<BankAccount>, BankAccountXmlRepository>();
            unityContainer.RegisterType<IRepository<BankAccount, string>, BankAccountXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperation>, BankOperationXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperation, int>, BankOperationXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperationType>, BankOperationTypeXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperationType, string>, BankOperationTypeXmlRepository>();
            unityContainer.RegisterType<IRepository<BankStatement>, BankStatementXmlRepository>();
            unityContainer.RegisterType<IRepository<BankStatement, int>, BankStatementXmlRepository>();
            
            //Context
            unityContainer.RegisterType<IContext, XmlContext>();
            unityContainer.RegisterType<IXmlSaveHandler, XmlSaveHandler>();
        }
    }   
}
