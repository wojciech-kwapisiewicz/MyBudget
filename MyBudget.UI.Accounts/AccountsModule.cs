using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Accounts
{
    public class AccountsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public AccountsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            //Navigable elements
            _container.RegisterType<object, AccountView>(typeof(AccountView).FullName);
            _container.RegisterType<object, AccountsView>(typeof(AccountsView).FullName);
            _container.RegisterType<object, OperationsView>(typeof(OperationsView).FullName);
            _container.RegisterType<object, StatementsView>(typeof(StatementsView).FullName);
        }
    }
}
