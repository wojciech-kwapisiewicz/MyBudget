using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Popups
{
    public class MessageBoxNotification : IMessageBoxNotification
    {
        public MessageBoxButton Buttons { get; set; }

        public MessageBoxResult Result { get; set; }

        public Action<MessageBoxResult> Continuation { get; set; }

        public object Content { get; set; }

        public string Title { get; set; }
    }
}
