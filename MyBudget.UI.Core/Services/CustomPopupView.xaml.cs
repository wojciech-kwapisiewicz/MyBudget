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

namespace MyBudget.UI.Core.Services
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

        public INotification Notification
        {
            get { return (INotification)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Notification.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(INotification), typeof(CustomPopupView), new PropertyMetadata(null));
        
        public Action FinishInteraction { get; set; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            ((IMessageBoxNotification)Notification).Result = MessageBoxResult.OK;
            if(FinishInteraction!=null)
            {
                FinishInteraction();
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            ((IMessageBoxNotification)Notification).Result = MessageBoxResult.Cancel;
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }
    }
}
