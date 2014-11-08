using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Core.InMemoryPersistance;
using MyBudget.Core.Model;
using MyBudget.Core.Persistance;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MyBudget.Core.UnitTests.Persistance
{
    [TestFixture]
    public class XmlContextTests
    {
        private static XmlRepositoryFactory GetRepoFactory()
        {
            BankAccountXmlRepository ba = new BankAccountXmlRepository();
            BankOperationTypeXmlRepository bt = new BankOperationTypeXmlRepository();
            BankStatementXmlRepository bs = new BankStatementXmlRepository();
            BankOperationXmlRepository bo = new BankOperationXmlRepository(ba, bt, bs);
            return new XmlRepositoryFactory(ba, bt, bs, bo);
        }

        [Test]
        public void GivenXmlContext_WhenQueriedForProperRepo_ItIsReturned()
        {           
            //Given
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            saveMock.Setup(a => a.Load()).Returns(new XElement("savedData"));
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //When
            var r1 = context.GetRepository<BankAccountXmlRepository>();
            var r2 = context.GetRepository<BankOperationTypeXmlRepository>();
            var r3 = context.GetRepository<BankOperationXmlRepository>();
            var r4 = context.GetRepository<BankStatementXmlRepository>();

            //Then
            Assert.IsNotNull(r1);
            Assert.IsNotNull(r2);
            Assert.IsNotNull(r3);
            Assert.IsNotNull(r4);
        }

        [Test]
        public void GivenEmptyContext_WhenBankAccountAddedAndSaved_ThenXElementContainsAccount()
        {
            //Given
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            saveMock.Setup(a => a.Load()).Returns(new XElement("savedData"));
            XElement savedXElement = null;
            saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => savedXElement = c);
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //When
            var accountsRepository = context.GetRepository<IRepository<BankAccount>>();
            accountsRepository.Add(new BankAccount() { Number = "A", Name = "B", Description = "C" });

            Assert.IsTrue(context.SaveChanges());

            //Then
            string expectedXml = ManifestStreamReaderHelper
                .ReadEmbeddedResource(typeof(XmlContextTests), "expectedOutput_savedData.xml");
            saveMock.Verify(a => a.Save(It.IsAny<XElement>()));
            Assert.AreEqual(expectedXml, savedXElement.ToString());
        }

        [Test]
        public void GivenXmlWithAccount_WhenContextCreated_ThenItContainsThisAccount()
        {
            //Given
            string xmlToLoad = ManifestStreamReaderHelper
                .ReadEmbeddedResource(typeof(XmlContextTests), "input_saved1Account.xml");
            XElement el = XElement.Parse(xmlToLoad);
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            saveMock.Setup(a => a.Load()).Returns(el);

            //When
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //Then
            var repository = context.GetRepository<IRepository<BankAccount>>();
            Assert.IsTrue(repository.GetAll().Any(a =>
                a.Name == "AccountName1" &&
                a.Number == "AccountNumber2" &&
                a.Description == "AccountDescription3" &&
                a.Id == "AccountNumber2"));
        }

        [Test]
        public void GivenEmptyContext_WhenSavedState_ThenXmlAsExpected()
        {
            //Given
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            saveMock.Setup(a => a.Load()).Returns(new XElement("savedData"));
            XElement savedXElement = null;
            saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => savedXElement = c);
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //When
            var account = new BankAccount() { Number = "A", Name = "B", Description = "C" };
            var accountsRepository = context.GetRepository<IRepository<BankAccount>>();
            accountsRepository.Add(account);
            
            var opType = new BankOperationType(){Name = "xx"};
            var opTypeRepository = context.GetRepository<IRepository<BankOperationType>>();
            opTypeRepository.Add(opType);


            var ops = new List<BankOperation>()
            {
                new BankOperation(){Id=1,Description="A",BankAccount=account, Type = opType},
                new BankOperation(){Id=2,Description="C",BankAccount =account, Type = opType}
            };

            var opRepository = context.GetRepository<IRepository<BankOperation>>();
            foreach (var item in ops)
            {
                opRepository.Add(item);
            }

            var statement = new BankStatement()
            {
                FileName = "A",
                LoadTime = new DateTime(),
                Operations = ops
            };
            var statementRepository = context.GetRepository<IRepository<BankStatement>>();
            statementRepository.Add(statement);
            
            Assert.IsTrue(context.SaveChanges());

            //Then
            string expectedXml = ManifestStreamReaderHelper
                .ReadEmbeddedResource(typeof(XmlContextTests), "expectedOutput_fullSavedModel.xml");
            saveMock.Verify(a => a.Save(It.IsAny<XElement>()));
            Assert.AreEqual(expectedXml, savedXElement.ToString());

            Mock<IXmlSaveHandler> saveMock2 = new Mock<IXmlSaveHandler>();
            saveMock2.Setup(a => a.Load()).Returns(new XElement(savedXElement));

            var context2 = new XmlContext(saveMock.Object, GetRepoFactory());
        }
    }
}
