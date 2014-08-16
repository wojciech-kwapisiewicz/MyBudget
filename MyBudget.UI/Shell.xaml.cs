using Microsoft.Win32;
using MyBudget.Core;
using MyBudget.UI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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

namespace MyBudget.UI
{
    /// <summary>
    /// Interaction logic for MainWindowShell.xaml
    /// </summary>
    public partial class Shell : Window, INotifyPropertyChanged
    {
        public Shell(LocalizedReflection localizedReflection)
        {
            _localizedReflection = localizedReflection;
            InitializeComponent();
        }


        LocalizedReflection _localizedReflection;
        public LocalizedReflection LocalizedReflection
        {
            get
            {
                return _localizedReflection;
            }
            set
            {
                _localizedReflection = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LocalizedReflection"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
