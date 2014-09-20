using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Main
{
    public class LanguageSetupViewModel : BindableBase
    {
        public LanguageSetupViewModel()
        {
            var a = WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture;
            var b = WPFLocalizeExtension.Providers.ResxLocalizationProvider.Instance.AvailableCultures;
            CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        public CultureInfo CurrentCulture { get; set; }
        public IEnumerable<CultureInfo> AvailableCultures
        {
            get
            {
                return WPFLocalizeExtension.Providers.ResxLocalizationProvider.Instance.AvailableCultures;
                //return CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            }
        }
    }
}
