using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Main
{
    public class MainModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public MainModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<object, MainContentView>(typeof(MainContentView).ToString());
            _container.RegisterType<object, WelcomePageView>(typeof(WelcomePageView).ToString());

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigation, typeof(MainNavigationView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(WelcomePageView));
            //_regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(MainContentView));
        }
    }
}
