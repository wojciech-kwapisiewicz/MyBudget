using java.io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Pdf
{
    public class JavaIoWrapper : InputStream
    {
        private Stream _underlyingStream;

        public JavaIoWrapper(Stream stream)
        {
            _underlyingStream = stream;
        }

        public override int read()
        {
            return _underlyingStream.ReadByte();
        }
    }
}
