using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Popups
{
    public class ShowMessageBoxEventParameter
    {
        public string Caption { get; set; }
        public string Content { get; set; }
        public MessageBoxButton Buttons { get; set; }
        public Action<MessageBoxResult> Continuation { get; set; }
    }
}
