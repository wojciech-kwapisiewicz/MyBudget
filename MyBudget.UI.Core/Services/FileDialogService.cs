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
        public OpenFileResult OpenFile(string customFilter, bool multiselect)
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
            dialog.Multiselect = multiselect;
            
            List<OpenedFile> openedFiles = new List<OpenedFile>();
            if (dialog.ShowDialog() == true)
            {
                string[] fileNames = dialog.FileNames;

                foreach (var filePath in fileNames)
                {
                    string fileName = Path.GetFileName(filePath);
                    Stream stream = File.OpenRead(filePath);
                    openedFiles.Add(new OpenedFile(stream, fileName));
                }
            }

            return new OpenFileResult(openedFiles);
        }
    }
}
