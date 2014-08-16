using MyBudget.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class CustomDescription
    {
        public int Id { get; set; }

        [LocalDescription("Kategoria", Language.Polish)]
        public string Category { get; set; }
        [LocalDescription("Podkategoria", Language.Polish)]
        public string SubCategory { get; set; }
        [LocalDescription("Numer karty", Language.Polish)]
        public string CardNumber { get; set; }
    }
}
