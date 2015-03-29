using Microsoft.Practices.Unity;
using MyBudget.OperationsLoading.MilleniumAccount;
using MyBudget.OperationsLoading.PkoBpAccount;
using MyBudget.OperationsLoading.PkoBpCreditCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Configuration
{
    public class OperationsLoadingUnityConfiguration
    {
        public void Configure(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<IRepositoryHelper, RepositoryHelper>();

            //Parsers
            unityContainer.RegisterType<IParser, PkoBpParser>("Pko BP standard account parser");
            unityContainer.RegisterType<IParser, MilleniumParser>("Millenium account parser");
            unityContainer.RegisterType<IParser, PkoBpCreditCardUnclearedParser>("Pko BP credit card parser");
            unityContainer.RegisterType<IParser, PkoBpCreditClearedParser>("Pko BP credit card cleared parser");
        }
    }   
}
