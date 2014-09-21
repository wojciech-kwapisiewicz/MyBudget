using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core
{
    public static class ResourceManagerHelper
    {
        public static string GetTranslation(Type type, string propertyName)
        {
            ResourceManager rm = new ResourceManager(
                type.Namespace + ".Translations",
                System.Reflection.Assembly.GetAssembly(type));
            string nameFromResource = rm.GetString(type.Name + '_' + propertyName);
            return nameFromResource ?? propertyName;
        }
    }
}
