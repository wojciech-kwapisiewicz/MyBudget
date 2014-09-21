using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Localization
{
    public class LanguageSettings : ILanguageSettings
    {
        public CultureInfo CurrentCulture
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture;
            }
            set
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = value;
            }
        }

        public IEnumerable<CultureInfo> AvailableCultures
        {
            get
            {
                return CultureInfo.GetCultures(CultureTypes.SpecificCultures | CultureTypes.NeutralCultures);
            }
        }

        public CultureInfo CurrentUICulture
        {
            get
            {
                return WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture;
            }
            set
            {
                WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = value;
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;
            }
        }

        public IEnumerable<CultureInfo> AvailableUICultures
        {
            get
            {
                return WPFLocalizeExtension.Providers.ResxLocalizationProvider.Instance.AvailableCultures;
            }
        }
    }
}
