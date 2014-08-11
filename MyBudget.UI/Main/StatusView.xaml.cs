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

namespace MyBudget.UI.Main
{
    /// <summary>
    /// Interaction logic for StatusView.xaml
    /// </summary>
    public partial class StatusView : UserControl
    {
        private IRegionNavigationJournal _journal;
        public StatusView(IRegionNavigationJournal journal)
        {
            _journal = journal;
            InitializeComponent();
        }
    }
}
