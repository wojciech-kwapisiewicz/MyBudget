using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core
{
    public static class ReflectionHelper
    {
        public static IEnumerable<string> GetPropertiesNames(this Type type)
        {
            return type.GetProperties().Select(a => a.Name);
        }

        public static object GetPropertyValue<T>(this object obj, string propertyName)
        {
            return typeof(T).GetProperties().Single(a => a.Name == propertyName).GetValue(obj);
        }
    }
}
