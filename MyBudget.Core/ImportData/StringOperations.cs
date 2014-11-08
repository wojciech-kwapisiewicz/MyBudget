using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.ImportData
{
    public static class StringOperations
    {
        public static string RemoveLine(this string input)
        {
            int newLine = input.IndexOf("\r\n");
            return input.Substring(newLine + 2);
        }

        public static string RemoveLine(this string input, int no)
        {
            for (int i = 0; i < no; i++)
            {
                input = RemoveLine(input);
            }
            return input;
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string ReplaceLast(this string text, string search, string replace)
        {
            int pos = text.LastIndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
