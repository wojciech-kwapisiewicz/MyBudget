using MyBudget.UI.Core;
using System.ComponentModel;
using System.Windows;

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
