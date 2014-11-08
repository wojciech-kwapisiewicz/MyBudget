using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Pdf
{
    public class PdfReader
    {
        public string GetStringFromPdfStream(Stream stream)
        {
            PDDocument doc = null;
            try
            {
                doc = PDDocument.load(new JavaIoWrapper(stream));
                PDFTextStripper stripper = new PDFTextStripper();
                return stripper.getText(doc);
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
        }
    }
}
