using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Services
{
    public class OpenFileResult : IDisposable
    {
        public OpenFileResult(IEnumerable<OpenedFile> openedFiles)
        {
            OpenedFiles = openedFiles;
        }

        public IEnumerable<OpenedFile> OpenedFiles { get; private set; }

        public void Dispose()
        {
            foreach (var item in OpenedFiles)
            {
                item.Stream.Dispose();
            }
        }
    }
}
