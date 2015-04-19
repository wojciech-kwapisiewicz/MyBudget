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

namespace MyBudget.UI.Core.Controls
{    
    /// <summary>
    /// Interaction logic for DateRangeControl.xaml
    /// </summary>
    public partial class DateRangeControl : UserControl
    {
        public DateRangeControl()
        {
            byMonth = (a) =>
            {
                return a.Month == FilterMonth.Month && a.Year == FilterMonth.Year;
            };
            byRange = (a) =>
            {
                return a.Date >= StartDate.Date && a.Date <= EndDate.Date;
            };
            InitializeComponent();
            Action filterInitializer = () => SyncFilterInternal(this);
                //() => FilteringFunction = byRange;
            this.Dispatcher.BeginInvoke(filterInitializer);
            MainWrapper.DataContext = this;            
        }

        public Func<DateTime, bool> FilteringFunction
        {
            get { return (Func<DateTime, bool>)GetValue(FilteringFunctionProperty); }
            set { SetValue(FilteringFunctionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilteringFunction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilteringFunctionProperty =
            DependencyProperty.Register("FilteringFunction", typeof(Func<DateTime, bool>), typeof(DateRangeControl));

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Now.Date.AddMonths(-1), SyncFilter));

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Now.Date.AddMonths(1), SyncFilter));

        public DateTime FilterMonth
        {
            get { return (DateTime)GetValue(FilterMonthProperty); }
            set { SetValue(FilterMonthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterMonthProperty =
            DependencyProperty.Register("FilterMonth", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Now.Date, SyncFilter));
        
        public DateRangeType RangeType
        {
            get { return (DateRangeType)GetValue(RangeTypeProperty); }
            set { SetValue(RangeTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeTypeProperty =
            DependencyProperty.Register("RangeType", typeof(DateRangeType), typeof(DateRangeControl), new PropertyMetadata(DateRangeType.ByMonth, SyncFilter));

        private static void SyncFilter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var currentControl = d as DateRangeControl;

            SyncFilterInternal(currentControl);
        }

        private static void SyncFilterInternal(DateRangeControl currentControl)
        {
            switch (currentControl.RangeType)
            {
                case DateRangeType.ByMonth:
                    currentControl.FilteringFunction = currentControl.byMonth;
                    break;
                case DateRangeType.ByRange:
                    currentControl.FilteringFunction = currentControl.byRange;
                    break;
                default:
                    break;
            }
        }

        private static Func<DateTime, bool> _allIn = (a) => true;

        private Func<DateTime, bool> byMonth;

        private Func<DateTime, bool> byRange;
    }
}
