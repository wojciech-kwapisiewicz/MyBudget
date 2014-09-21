using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Localization
{
    public interface ILanguageSettings
    {
        CultureInfo CurrentCulture { get; set; }
        IEnumerable<CultureInfo> AvailableCultures { get; } 
        CultureInfo CurrentUICulture { get; set; }
        IEnumerable<CultureInfo> AvailableUICultures { get; } 
    }
}
