using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core
{
    public static class CustomCompareStrings
    {
        public static bool AreEqual(string a, string b)
        {
            return (a ?? string.Empty) == (b ?? string.Empty);
        }
    }
}
