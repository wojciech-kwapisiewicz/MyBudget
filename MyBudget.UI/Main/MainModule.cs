using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using MyBudget.UI.Core;
using MyBudget.UI.Core.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            _container.RegisterType<ApplicationSettings>(
                new ContainerControlledLifetimeManager());

            //Navigable elements
            _container.RegisterType<object, WelcomePageView>(typeof(WelcomePageView).FullName);

            //Starting regions registration
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigation, typeof(MainNavigationView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainContent, typeof(WelcomePageView));

            //Configuration
            _container.RegisterType<object, LanguageSetupView>(typeof(LanguageSetupView).FullName);

            //Services
            _container.RegisterType<IResolveClassificationConflicts, ResolveClassificationConflicts>();

            var langSettings = _container.Resolve<ApplicationSettings>();
            System.Threading.Thread.CurrentThread.CurrentCulture = langSettings.InputLanguage.Culture;
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = langSettings.Language.Culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = langSettings.Language.Culture;
            
        }
    }
}
