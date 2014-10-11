using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using MyBudget.Core;
using MyBudget.UI.Accounts;
using MyBudget.UI.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            RegisterModule(typeof(MainModule));
            RegisterModule(typeof(AccountsModule));
        }

        protected override void ConfigureContainer()
        {
            new CoreUnityConfiguration().Configure(base.Container);
            base.ConfigureContainer();
        }

        private void RegisterModule(Type mainModuleType)
        {
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = mainModuleType.Name,
                ModuleType = mainModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
    }
}
