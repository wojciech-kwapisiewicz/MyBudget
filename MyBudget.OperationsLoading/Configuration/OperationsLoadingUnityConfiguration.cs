﻿using Microsoft.Practices.Unity;
using MyBudget.OperationsLoading.BgzBnpParibasCsv;
using MyBudget.OperationsLoading.BnpParibasXlsx;
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
            Func<IUnityContainer, object> createBnpXlsxChain = container =>
                new CardHandler(container.Resolve<ParseHelper>(), container.Resolve<IRepositoryHelper>(),
                new BlikHandler(container.Resolve<ParseHelper>(),
                new DefaultHandler(container.Resolve<ParseHelper>())));
            unityContainer.RegisterType<IOperationHandler>(new InjectionFactory(createBnpXlsxChain));
            unityContainer.RegisterType<IParser, BnpParibasXslxParser>("BNP Paribas account parser");

            unityContainer.RegisterType<IParser, MilleniumParser>("Millenium account parser");

            unityContainer.RegisterType<IParser, PkoBpParser>("Pko BP standard account parser");

            unityContainer.RegisterType<IParser, PkoBpCreditClearedParser>("Pko BP credit card cleared parser");

            #region Legacy parser
            unityContainer.RegisterType<IParser, PkoBpCreditCardUnclearedParser>("Pko BP credit card parser");
            
            Func<IUnityContainer, object> createBnpParibasCsvChain = container =>
                new WyplataBankomat(container.Resolve<IRepositoryHelper>(), container.Resolve<ParseHelper>(),
                new OperacjaKarta(container.Resolve<IRepositoryHelper>(), container.Resolve<ParseHelper>(),
                new Przelew(container.Resolve<IRepositoryHelper>(),
                new PrzelewPrzychodzacy(container.Resolve<IRepositoryHelper>(), container.Resolve<ParseHelper>(),
                new PrzelewWychodzacy(container.Resolve<IRepositoryHelper>(),
                new InnaOperacja(container.Resolve<IRepositoryHelper>()))))));
            unityContainer.RegisterType<IFillOperationFromDescriptionChain>(new InjectionFactory(createBnpParibasCsvChain));
            unityContainer.RegisterType<IParser, BgzBnpParibasCsvParser>("BGZ BPN Paribas account parser");
            #endregion Legacy parser
        }
    }
}
