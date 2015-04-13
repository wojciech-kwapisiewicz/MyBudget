using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Core.Persistance;
using MyBudget.Model;
using MyBudget.XmlPersistance;
using MyBudget.XmlPersistance.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MyBudget.XmlPersistance.Tests
{
    [TestFixture]
    public abstract class XmlContextTestsBase
    {
        internal XmlRepositoryFactory GetRepoFactory()
        {
            BankAccountXmlRepository ba = new BankAccountXmlRepository();
            BankOperationTypeXmlRepository bt = new BankOperationTypeXmlRepository();
            BankStatementXmlRepository bs = new BankStatementXmlRepository();
            BankOperationXmlRepository bo = new BankOperationXmlRepository(ba, bt, bs);
            ClassificationDefinitionXmlRepository cr = new ClassificationDefinitionXmlRepository();
            return new XmlRepositoryFactory(ba, bt, bs, bo, cr);
        }

        internal XmlContext GetNewContext()
        {
            return new XmlContext(_saveMock.Object, GetRepoFactory());
        }

        internal Mock<IXmlSaveHandler> _saveMock;
        internal XElement _savedContext;

        [SetUp]
        public virtual void SetUp()
        {
            this._savedContext = new XElement("savedData");
            this._saveMock = new Mock<IXmlSaveHandler>();
            this._saveMock.Setup(a => a.Load()).Returns(() => _savedContext);
            this._saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => this._savedContext = c);
        }
    }
}
