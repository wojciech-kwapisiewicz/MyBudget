using MyBudget.Core.DataContext;
using MyBudget.Core.Model;
using MyBudget.Core.Persistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MyBudget.Core.InMemoryPersistance
{
    public class XmlContext : IContext
    {
        RepositoryFactory rf = new RepositoryFactory();

        public T GetRepository<T>() where T : IRepository
        {
            return rf.GetRepository<T>();
        }

        public XmlContext()
        {

        }

        public XmlContext(XElement dataToLoad)
        {
            XElement accountsElement = dataToLoad.Element("ArrayOfBankAccount");
            if (accountsElement != null)
            {
                rf.GetRepository<BankAccountXmlRepository>().Load(accountsElement);
            }
        }


        public bool SaveChanges()
        {
            XElement el = new XElement("savedData");
            el.Add(rf.GetRepository<BankAccountXmlRepository>().Save());

            return true;
        }

    }
}
