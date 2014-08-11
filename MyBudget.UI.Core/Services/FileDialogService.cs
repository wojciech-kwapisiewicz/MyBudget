using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Services
{
    public class FileDialogService
    {
        public Stream OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Xml Files (.xml)|*.xml|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {
                return dialog.OpenFile();
            }
            return null;
        }
    }
}
