using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBudget.Core.Persistance
{
    public interface IXmlSaveHandler
    {
        XElement Load();
        void Save(XElement state);
    }
}
