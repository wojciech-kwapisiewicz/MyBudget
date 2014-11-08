using Microsoft.Practices.Unity;
using MyBudget.Core.DataContext;
using MyBudget.Core.ImportData;
using MyBudget.Core.InMemoryPersistance;
using MyBudget.Core.Model;
using MyBudget.Core.Persistance;
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
            unityContainer.RegisterType<BankAccountXmlRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankOperationXmlRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankOperationTypeXmlRepository>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<BankStatementXmlRepository>(new ContainerControlledLifetimeManager());

            //unityContainer.RegisterType<IContext, InMemoryContext>();
            unityContainer.RegisterType<IContext, XmlContext>();
            unityContainer.RegisterType<IXmlSaveHandler, XmlSaveHandler>();
            unityContainer.RegisterType<IParseHelper, ParseHelper>();

            //Parsers
            unityContainer.RegisterType<IParser, PkoBpParser>("Pko BP standard account parser");
            unityContainer.RegisterType<IParser, MilleniumParser>("Millenium account parser");
            unityContainer.RegisterType<IParser, PkoBpCreditCardUnclearedParser>("Pko BP credit card parser");
            unityContainer.RegisterType<IParser, PkoBpCreditClearedParser>("Pko BP credit card cleared parser");

            unityContainer.RegisterType<IRepository<BankAccount>, BankAccountXmlRepository>();
            unityContainer.RegisterType<IRepository<BankAccount, string>, BankAccountXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperation>, BankOperationXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperation, int>, BankOperationXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperationType>, BankOperationTypeXmlRepository>();
            unityContainer.RegisterType<IRepository<BankOperationType, string>, BankOperationTypeXmlRepository>();
            unityContainer.RegisterType<IRepository<BankStatement>, BankStatementXmlRepository>();
            unityContainer.RegisterType<IRepository<BankStatement, int>, BankStatementXmlRepository>();
        }
    }   
}
