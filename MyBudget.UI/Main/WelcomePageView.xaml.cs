using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
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
using Microsoft.Practices.Prism.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using MyBudget.UI.Core.Services;
using Microsoft.Practices.Prism.PubSubEvents;

namespace MyBudget.UI.Main
{
    public class WelcomPageViewModel : BindableBase
    {
        private string InnerText = "Inner text";
        private string InnerCaption = "Inner caption";

        private IEventAggregator _eventAggregator;

        public WelcomPageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            ShowStandardMsgBox = new DelegateCommand(() => MessageBox.Show(
                InnerText, 
                InnerCaption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information,
                MessageBoxResult.No));
            ShowToolkitMsgBox = new DelegateCommand(ShowToolkig);
        }

        private object _Status;
        public object Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        private object _Selected;
        public object Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }

        public ICommand ShowStandardMsgBox { get; set; }
        public ICommand ShowToolkitMsgBox { get; set; }

        void ShowToolkig()
        {
            ShowMessageBoxEventParameter parameter = new ShowMessageBoxEventParameter()
            {
                Buttons = MessageBoxButton.OKCancel,
                Header = "???",
                Content = "!!!!",
                Continuation = Mmm
            };
            _eventAggregator.GetEvent<ShowMessageBoxEvent>().Publish(parameter);
            //System.Windows.Style style = new System.Windows.Style();
            //style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.YesButtonContentProperty, "Yes, FTW!"));
            //style.Setters.Add(new Setter(Xceed.Wpf.Toolkit.MessageBox.NoButtonContentProperty, "Omg, no"));

            //Xceed.Wpf.Toolkit.MessageBox.Show(
            //    InnerText,
            //    InnerCaption,
            //    MessageBoxButton.YesNo,
            //    MessageBoxImage.Information,
            //    MessageBoxResult.No,
            //    style);
        }


        void Mmm(MessageBoxResult result)
        {
            MessageBox.Show("wow1234");
        }
    }


    /// <summary>
    /// Interaction logic for WelcomePageView.xaml
    /// </summary>
    public partial class WelcomePageView : UserControl, IRegionMemberLifetime
    {
        public WelcomePageView(WelcomPageViewModel view)
        {
            this.DataContext = view;
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }            
    }
}
