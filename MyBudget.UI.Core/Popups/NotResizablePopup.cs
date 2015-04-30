using Microsoft.Practices.Prism.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyBudget.UI.Core.Popups
{
    public class NotResizablePopup : PopupWindowAction
    {
        protected override Window GetWindow(INotification notification)
        {
            var window = base.GetWindow(notification);
            window.ResizeMode = ResizeMode.NoResize;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            return window;
        }

        protected override void PrepareContentForWindow(INotification notification, Window wrapperWindow)
        {
            base.PrepareContentForWindow(notification, wrapperWindow);
        }

        protected override Freezable CreateInstanceCore()
        {
            return base.CreateInstanceCore();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
        }
    }
}
