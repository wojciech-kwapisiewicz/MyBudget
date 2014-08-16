using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Localization
{
    public class LocalDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public string Language { get; set; }
        public LocalDescriptionAttribute(string description, string language)
        {
            Description = description;
            Language = language;
        }
    }
}
