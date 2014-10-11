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
        ApplicationSettings _applicationSettings;

        public LanguageSetupViewModel(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public CultureMapping Language
        {
            get
            {
                return _applicationSettings.Language;
            }
            set
            {
                _applicationSettings.Language = value;
            }
        }

        public CultureMapping InputLanguage
        {
            get
            {
                return _applicationSettings.InputLanguage;
            }
            set
            {
                _applicationSettings.InputLanguage = value;
            }
        }

        public IEnumerable<CultureMapping> AvailableInputLanguages
        {
            get
            {
                return _applicationSettings.AvailableInputLanguages;
            }
        }

        public IEnumerable<CultureMapping> AvailableLanguages
        {
            get
            {
                return _applicationSettings.AvailableLanguages;
            }
        }
    }
}
