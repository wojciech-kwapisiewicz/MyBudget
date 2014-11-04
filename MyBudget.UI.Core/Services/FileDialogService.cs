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
        public OpenFileResult OpenFile(string customFilter)
        {
            List<string> baseFilters = new List<string>();
            if (!string.IsNullOrEmpty(customFilter))
            {
                baseFilters.Add(customFilter);
            }
            baseFilters.Add(Resources.Translations.FilterAllSupported);
            baseFilters.Add(Resources.Translations.FilterAllFiles);

            string delimiter = "|";
            string filter = baseFilters.Aggregate((i, j) => i + delimiter + j);

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
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
