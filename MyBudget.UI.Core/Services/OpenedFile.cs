using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.UI.Core.Services
{
    public class OpenedFile : IDisposable
    {
        public OpenedFile(Stream stream, string fileName)
        {
            FileName = fileName;
            Stream = stream;
        }

        public string FileName { get; private set; }
        public Stream Stream { get; private set; }

        public void Dispose()
        {
            this.Stream.Dispose();
        }
    }
}
