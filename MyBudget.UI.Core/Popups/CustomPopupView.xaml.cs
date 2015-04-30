using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBudget.UI.Core.Popups
{
    /// <summary>
    /// Interaction logic for CustomPopupView.xaml
    /// </summary>
    public partial class CustomPopupView : UserControl, IInteractionRequestAware
    {
        public CustomPopupView()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        private IMessageBoxNotification MsgBoxNotification
        {
            get
            {
                return (IMessageBoxNotification)Notification;
            }
        }

        public INotification Notification
        {
            get { return (INotification)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Notification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(INotification), typeof(CustomPopupView), new PropertyMetadata(null));
        
        public Action FinishInteraction { get; set; }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            if (MsgBoxNotification.Buttons == MessageBoxButton.YesNo ||
                MsgBoxNotification.Buttons == MessageBoxButton.YesNoCancel)
            {
                MsgBoxNotification.Result = MessageBoxResult.Yes;
                if (FinishInteraction != null)
                {
                    FinishInteraction();
                }
            }
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            if (MsgBoxNotification.Buttons == MessageBoxButton.YesNo ||
                MsgBoxNotification.Buttons == MessageBoxButton.YesNoCancel)
            {

                MsgBoxNotification.Result = MessageBoxResult.No;
                if (FinishInteraction != null)
                {
                    FinishInteraction();
                }
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (MsgBoxNotification.Buttons == MessageBoxButton.OK ||
                MsgBoxNotification.Buttons == MessageBoxButton.OKCancel)
            {
                MsgBoxNotification.Result = MessageBoxResult.OK;
                if (FinishInteraction != null)
                {
                    FinishInteraction();
                }
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (MsgBoxNotification.Buttons == MessageBoxButton.OKCancel ||
                MsgBoxNotification.Buttons == MessageBoxButton.YesNoCancel)
            {
                MsgBoxNotification.Result = MessageBoxResult.Cancel;
                if (FinishInteraction != null)
                {
                    FinishInteraction();
                }
            }
        }
    }
}
