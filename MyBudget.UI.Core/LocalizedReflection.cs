using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core
{
    public class LocalizedReflection
    {
        ICurrentLanguage _currentLanguage;

        public LocalizedReflection(ICurrentLanguage currentLanguage)
        {
            _currentLanguage = currentLanguage;
        }

        public IEnumerable<string> GetPropertiesNames(Type type)
        {
            foreach (var property in type.GetProperties().FilterOutDontDisplay())
            {
                var localizationAttribute = property
                    .GetCustomAttributes(typeof(LocalDescriptionAttribute), false)
                    .Cast<LocalDescriptionAttribute>()
                    .SingleOrDefault(a => a.Language == _currentLanguage.Language);

                yield return localizationAttribute == null ? property.Name : localizationAttribute.Description;
            }
        }

        public object GetPropertyValue<T>(object obj, string propertyName)
        {
            PropertyInfo pi = typeof(T).GetProperties().FilterOutDontDisplay()
                .SingleOrDefault(a => a
                    .GetCustomAttributes(typeof(LocalDescriptionAttribute), false)
                    .Cast<LocalDescriptionAttribute>()
                    .Any(b => b.Language == _currentLanguage.Language && b.Description == propertyName));

            if (pi == null)
            {
                pi = typeof(T).GetProperties().FilterOutDontDisplay().Single(a => a.Name == propertyName);
            }

            return pi.GetValue(obj);
        }
    }
}
