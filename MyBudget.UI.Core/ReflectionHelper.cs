using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> FilterOutDontDisplay(this IEnumerable<PropertyInfo> piList)
        {
            return piList.Where(a => !Attribute.IsDefined(a, typeof(DontDisplayAttribute)));
        }
    }
}
