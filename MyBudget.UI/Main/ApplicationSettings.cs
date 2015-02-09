using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Main
{
    public class ApplicationSettings : BindableBase
    {
        public const string LanguageKey = "language";
        public const string InputLanguageKey = "inputLanguage";

        public ApplicationSettings()
        {
            string languageName = ResolveLanguageName();
            AvailableLanguages = InitializeAvailableLanguages();
            Language = AvailableLanguages.First(a => a.CultureName == languageName);

            string inputLanguageName = ResolveInputLanguage();
            AvailableInputLanguages = InitializeAvailableInputLanguages(inputLanguageName);
            InputLanguage = AvailableInputLanguages.First(a => a.Culture.NativeName == inputLanguageName);
        }

        private IEnumerable<CultureMapping> InitializeAvailableLanguages()
        {
            return new List<CultureMapping>()
            {
                new CultureMapping()
                {
                    CultureName = "English",
                    Culture = CultureInfo.InvariantCulture
                },
                new CultureMapping()
                {
                    CultureName = "Polski",
                    Culture = CultureInfo.GetCultureInfo("pl")
                }
            };
        }

        private IEnumerable<CultureMapping> InitializeAvailableInputLanguages(string inputLanguageName)
        {
            IEnumerable<CultureInfo> inputMappings =
                CultureInfo.GetCultures(CultureTypes.NeutralCultures | CultureTypes.SpecificCultures)
                    .OrderBy(a => a.NativeName != inputLanguageName)
                    .ThenBy(b => b.Name != "pl")
                    .ThenBy(c => c.Name != "en" && c.Parent.Name != "en")
                    .ThenBy(d => d.EnglishName);

            return inputMappings
                .Select(x => new CultureMapping()
                    {
                        Culture = x,
                        CultureName = x.NativeName
                    }).ToArray();
        }

        private static string ResolveInputLanguage()
        {
            string inputLanguageName = ConfigurationManager.AppSettings[InputLanguageKey];
            if (string.IsNullOrEmpty(inputLanguageName))
            {
                inputLanguageName = System.Threading.Thread.CurrentThread.CurrentCulture.NativeName;
                //Initialize
            }
            return inputLanguageName;
        }

        private static string ResolveLanguageName()
        {
            string languageName = ConfigurationManager.AppSettings[LanguageKey];
            if (string.IsNullOrEmpty(languageName))
            {
                var uiCult = System.Threading.Thread.CurrentThread.CurrentUICulture;
                if (uiCult.Name == "pl" || uiCult.Parent.Name == "pl")
                {
                    languageName = "Polski";
                }
                else
                {
                    languageName = "English";
                }
                //Initialize
            }
            return languageName;
        }

        public string SaveFile
        {
            get
            {
                return ConfigurationManager.AppSettings["saveFile"];
            }
            set
            {
                ConfigurationManager.AppSettings["saveFile"] = value;
            }
        }

        public CultureMapping Language
        {
            get
            {
                string langName = ConfigurationManager.AppSettings[LanguageKey];
                return AvailableLanguages.First(a => a.CultureName == langName);
            }
            set
            {
                SetKeyInConfig(LanguageKey, value.CultureName);
                OnPropertyChanged(() => Language);
            }
        }

        private static void SetKeyInConfig(string key, string value)
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var set = configuration.AppSettings.Settings[key];
            if (set != null)
            {
                set.Value = value;
            }
            else
            {
                configuration.AppSettings.Settings.Add(new KeyValueConfigurationElement(key, value));
            }
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public IEnumerable<CultureMapping> AvailableLanguages { get; private set; }

        public CultureMapping InputLanguage
        {
            get
            {
                string langName = ConfigurationManager.AppSettings[InputLanguageKey];
                return AvailableInputLanguages.First(a => a.CultureName == langName);
            }
            set
            {
                SetKeyInConfig(InputLanguageKey, value.CultureName);
                OnPropertyChanged(() => Language);
            }
        }

        public IEnumerable<CultureMapping> AvailableInputLanguages { get; private set; }
    }
}
