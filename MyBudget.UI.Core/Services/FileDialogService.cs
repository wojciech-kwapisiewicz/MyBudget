using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Services
{
    public class OpenFileResult : IDisposable
    {
        public OpenFileResult(Stream stream, string fileName)
        {
            FileName = fileName;
            Stream = stream;
        }

        public string FileName { get; private set; }
        public Stream Stream { get; private set; }

        public void Dispose()
        {
            if (Stream != null)
            {
                Stream.Dispose();
            }
        }
    }

    public class FileDialogService
    {
        public OpenFileResult OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Xml Files (.xml)|*.xml|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.SafeFileName;
                Stream stream = dialog.OpenFile();
                return new OpenFileResult(stream, fileName);
            }

            return new OpenFileResult(null, null);
        }
    }
}
