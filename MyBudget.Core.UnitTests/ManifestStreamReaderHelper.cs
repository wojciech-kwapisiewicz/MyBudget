using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.UnitTests
{
    public static class ManifestStreamReaderHelper
    {
        public static string ReadEmbeddedResource(Assembly assembly, string resourceName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader stringReader = new StreamReader(
                stream,
                Encoding.GetEncoding("iso-8859-2")))
            {
                string result = stringReader.ReadToEnd();
                return result;
            }
        }

        public static string ReadEmbeddedResource(Type type, string relativeResource)
        {
            return ReadEmbeddedResource(type.Assembly, type.Namespace + "." + relativeResource);
        }
    }
}
