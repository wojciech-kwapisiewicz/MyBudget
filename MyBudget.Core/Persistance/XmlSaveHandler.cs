using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBudget.Core.Persistance
{
    public class XmlSaveHandler : IXmlSaveHandler
    {
        string s = "save.sav0";

        public XElement Load()
        {
            if(File.Exists(s))
            {
                return XElement.Load(s);
            }
            else
            {
                return new XElement("root");
            }
        }
        
        public void Save(XElement state)
        {
            state.Save(s);
        }
    }
}
