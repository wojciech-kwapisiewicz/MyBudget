using Microsoft.Practices.Prism.Mvvm;
using MyBudget.UI.Localization;
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
        ILanguageSettings _languageSettings;

        public LanguageSetupViewModel(ILanguageSettings languageSettings)
        {
            _languageSettings = languageSettings;
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return _languageSettings.CurrentCulture;
            }
            set
            {
                _languageSettings.CurrentCulture = value;
            }
        }

        public CultureInfo CurrentUICulture
        {
            get
            {
                return _languageSettings.CurrentUICulture;
            }
            set
            {
                _languageSettings.CurrentUICulture = value;
            }
        }

        public IEnumerable<CultureInfo> AvailableUICultures
        {
            get
            {
                return _languageSettings.AvailableUICultures;
            }
        }

        public IEnumerable<CultureInfo> AvailableCultures
        {
            get
            {
                return _languageSettings.AvailableCultures;
            }
        }
    }
}
