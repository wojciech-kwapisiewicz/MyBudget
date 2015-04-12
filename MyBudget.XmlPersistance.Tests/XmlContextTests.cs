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
            ClassificationRuleXmlRepository cr = new ClassificationRuleXmlRepository();
            return new XmlRepositoryFactory(ba, bt, bs, bo, cr);
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
            saveMock.Verify(a => a.Save(It.IsAny<XElement>()));
            Assert.IsNotNull(savedXElement.ToString());
        }

        [Test]
        public void GivenXmlWithAccount_WhenContextCreated_ThenItContainsThisAccount()
        {
            //Given
            XElement el = XElement.Parse(TestFiles.input_saved1Account);
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

            var opType = new BankOperationType() { Name = "xx" };
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
            saveMock.Verify(a => a.Save(It.IsAny<XElement>()));
            Assert.IsNotNull(savedXElement.ToString());

            Mock<IXmlSaveHandler> saveMock2 = new Mock<IXmlSaveHandler>();
            saveMock2.Setup(a => a.Load()).Returns(new XElement(savedXElement));

            var context2 = new XmlContext(saveMock.Object, GetRepoFactory());
        }

        [Test]
        public void GivenSomeContext_WhenNoChanges_ThenNoChangesReported()
        {
            //Given
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            XElement el = XElement.Parse(TestFiles.savedEmptyContext);
            saveMock.Setup(a => a.Load()).Returns(el);
            XElement savedXElement = null;
            saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => savedXElement = c);
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //Then
            Assert.IsFalse(context.DataHasChanged());
        }

        [Test]
        public void GivenSomeContext_WhenThereAreChanges_ThenChangesReported()
        {
            //Given
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            XElement el = XElement.Parse(TestFiles.savedEmptyContext);
            saveMock.Setup(a => a.Load()).Returns(el);
            XElement savedXElement = null;
            saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => savedXElement = c);
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //When
            var accountRepo = context.GetRepository<IRepository<BankAccount>>();
            accountRepo.Add(new BankAccount() { Description = "a", Name = "b", Number = "c" });

            //Then
            Assert.IsTrue(context.DataHasChanged());
        }

        [Test]
        public void GivenSomeContext_WhenChangesAreSaved_ThenNoChangesReported()
        {
            //Given
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            XElement el = XElement.Parse(TestFiles.savedEmptyContext);
            saveMock.Setup(a => a.Load()).Returns(el);
            XElement savedXElement = null;
            saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => savedXElement = c);
            var context = new XmlContext(saveMock.Object, GetRepoFactory());

            //When
            var accountRepo = context.GetRepository<IRepository<BankAccount>>();
            accountRepo.Add(new BankAccount() { Description = "a", Name = "b", Number = "c" });
            context.SaveChanges();

            //Then
            Assert.IsFalse(context.DataHasChanged());
        }

        [Test]
        public void GivenEmptyContext_WhenOperationsAddedAndSaved_ThenAllPublicFieldsAreStored()
        {
            //Given
            XElement savedXElement = new XElement("savedData");
            Mock<IXmlSaveHandler> saveMock = new Mock<IXmlSaveHandler>();
            saveMock.Setup(a => a.Load()).Returns(savedXElement);
            saveMock.Setup(b => b.Save(It.IsAny<XElement>()))
                .Callback<XElement>(c => savedXElement = c);
            var saveContext = new XmlContext(saveMock.Object, GetRepoFactory());

            //When
            var account = new BankAccount() { Number = "ACC1", Name = "Account1", Description = "DescAccount1" };
            var accountsRepository = saveContext.GetRepository<IRepository<BankAccount>>();
            accountsRepository.Add(account);

            var opType = new BankOperationType() { Name = "Type1" };
            var opTypeRepository = saveContext.GetRepository<IRepository<BankOperationType>>();
            opTypeRepository.Add(opType);

            var ops = new List<BankOperation>()
            {
                Initializer(1, account, opType, null),
                Initializer(2, account, opType, null),
                Initializer(3, account, opType, null)
            };

            var opRepository = saveContext.GetRepository<IRepository<BankOperation>>();
            foreach (var item in ops)
            {
                opRepository.Add(item);
            }

            var statement = new BankStatement()
            {
                FileName = "StatementFile",
                LoadTime = new DateTime(),
                Operations = ops
            };

            var statementRepository = saveContext.GetRepository<IRepository<BankStatement>>();
            statementRepository.Add(statement);

            Assert.IsTrue(saveContext.SaveChanges());

            //Then
            saveMock.Setup(a => a.Load()).Returns(savedXElement);
            var loadContext = new XmlContext(saveMock.Object, GetRepoFactory());
            var savedOperationRepository = loadContext.GetRepository<IRepository<BankOperation>>();
            Assert.AreEqual(3, savedOperationRepository.GetAll().Count());
            foreach (var item in savedOperationRepository.GetAll())
            {
                CheckOperation(item, account, opType);
            }

        }

        private void CheckOperation(BankOperation operation, BankAccount account, BankOperationType opType)
        {
            Assert.AreEqual(account.Id, operation.BankAccount.Id);
            Assert.AreEqual(opType.Id, operation.Type.Id);
            var properties = GetAutosetProperties();

            for (int propertyNumber = 0; propertyNumber < properties.Length; propertyNumber++)
            {
                var property = properties[propertyNumber];
                var expectedValue = GetValueForProperty(property.PropertyType,propertyNumber + 1,operation.Id);
                var currentValue = properties[propertyNumber].GetValue(operation);
                Assert.AreNotEqual(GetDefaultValue(property.PropertyType), currentValue,
                    string.Format("Property {0} has unexpected default value", property.Name));
                Assert.AreEqual(expectedValue, currentValue,
                    string.Format("Property {0} has unexpected value. Expected {1}, Current {2}.", property.Name, expectedValue, currentValue));
            }
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        private BankOperation Initializer(
            int id,
            BankAccount account,
            BankOperationType operationType,
            Card card)
        {
            var operation = new BankOperation()
            {
                Id = 1,
                BankAccount = account,
                Type = operationType,
                Card = card
            };

            var properties = GetAutosetProperties();

            for (int propertyNumber = 0; propertyNumber < properties.Length; propertyNumber++)
            {
                InitProperty(properties[propertyNumber], operation, propertyNumber + 1, id);
            }

            return operation;
        }

        private static PropertyInfo[] GetAutosetProperties()
        {
            string[] skippedFields = new string[]
            {
                "Id",
                "BankAccount",
                "Type",
                "Card"
            };

            var properties = typeof(BankOperation).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(a => !skippedFields.Contains(a.Name)).OrderBy(b => b.Name).ToArray();
            return properties;
        }

        private void InitProperty(PropertyInfo propertyInfo, BankOperation operation, int propertyNumber, int operationNumber)
        {
            var valueToSet = GetValueForProperty(propertyInfo.PropertyType, propertyNumber, operationNumber);
            propertyInfo.SetValue(operation, valueToSet);
        }

        private static object GetValueForProperty(Type objectType, int propertyNumber, int operationNumber)
        {
            object toSet = null;

            if (objectType == typeof(int))
            {
                toSet = propertyNumber * operationNumber;
            }
            else if (objectType == typeof(bool))
            {
                toSet = true;
            }
            else if (objectType == typeof(DateTime))
            {
                toSet = new DateTime(operationNumber, 1, propertyNumber);
            }
            else if (objectType == typeof(decimal))
            {
                toSet =  operationNumber + 0.01m * propertyNumber;
            }
            else if (objectType == typeof(string))
            {
                toSet = string.Format("StringValue_{0}_{1}", operationNumber, propertyNumber);
            }
            else
            {
                throw new ArgumentException("No such type handled");
            }

            return toSet;
        }
    }
}
