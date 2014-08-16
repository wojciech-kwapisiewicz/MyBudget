using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Localization
{
    public class CurrentLanguage : ICurrentLanguage
    {
        public string Language { get; set; }
    }
}
