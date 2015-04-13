using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MyBudget.XmlPersistance
{
    public class XmlContext : IContext
    {
        private XmlRepositoryFactory _repositoryFactory;
        private string _originalData = string.Empty;

        public T GetRepository<T>() where T : IRepository
        {
            return _repositoryFactory.GetRepository<T>();
        }

        IXmlSaveHandler _saveHandler;

        public XmlContext(IXmlSaveHandler saveHandler,
            XmlRepositoryFactory repositoryFactory)
        {
            if (saveHandler == null)
                throw new ArgumentNullException("saveHandler");
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _saveHandler = saveHandler;
            _repositoryFactory = repositoryFactory;

            XElement dataToLoad = _saveHandler.Load();
            _originalData = dataToLoad.ToString(SaveOptions.DisableFormatting);

            XElement accountsElement = dataToLoad.Element("ArrayOfBankAccount");           
            if (accountsElement != null)
            {
                _repositoryFactory.GetRepository<BankAccountXmlRepository>().Load(accountsElement);
            }
            XElement statementsElement = dataToLoad.Element("ArrayOfBankStatement");
            if (statementsElement != null)
            {
                _repositoryFactory.GetRepository<BankStatementXmlRepository>().Load(statementsElement);
            }
            XElement operationTypesElement = dataToLoad.Element("ArrayOfBankOperationType");
            if (operationTypesElement != null)
            {
                _repositoryFactory.GetRepository<BankOperationTypeXmlRepository>().Load(operationTypesElement);
            }
            XElement operationsElement = dataToLoad.Element("ArrayOfBankOperation");
            if (operationsElement != null)
            {
                _repositoryFactory.GetRepository<BankOperationXmlRepository>().Load(operationsElement);
            }
            XElement definitionsElement = dataToLoad.Element("ArrayOfClassificationDefinition");
            if (definitionsElement != null)
            {
                _repositoryFactory.GetRepository<ClassificationDefinitionXmlRepository>().Load(definitionsElement);
            }
        }

        public bool SaveChanges()
        {
            XElement el = GenerateDataToSave();
            _saveHandler.Save(el);
            _originalData = el.ToString(SaveOptions.DisableFormatting);
            return true;
        }

        private XElement GenerateDataToSave()
        {
            XElement el = new XElement("savedData");
            el.Add(_repositoryFactory.GetRepository<BankAccountXmlRepository>().Save());
            el.Add(_repositoryFactory.GetRepository<BankStatementXmlRepository>().Save());
            el.Add(_repositoryFactory.GetRepository<BankOperationTypeXmlRepository>().Save());
            el.Add(_repositoryFactory.GetRepository<BankOperationXmlRepository>().Save());
            el.Add(_repositoryFactory.GetRepository<ClassificationDefinitionXmlRepository>().Save());
            return el;
        }

        public bool DataHasChanged()
        {
            XElement el = GenerateDataToSave();
            string currentData = el.ToString(SaveOptions.DisableFormatting);
            return string.Equals(_originalData, currentData) == false;
        }
    }
}
