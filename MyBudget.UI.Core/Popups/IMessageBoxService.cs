using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Popups
{
    public interface IMessageBoxService
    {
        void ShowMessageBox(string caption, string conent, MessageBoxButton buttons, Action<MessageBoxResult> continuation);
    }
}
