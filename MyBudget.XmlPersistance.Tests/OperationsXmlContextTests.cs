using MyBudget.Core.DataContext;
using MyBudget.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.XmlPersistance.Tests
{
    [TestFixture]
    public class OperationsXmlContextTests : XmlContextTestsBase
    {
        [Test]
        public void GivenEmptyContext_WhenOperationsAddedAndSaved_ThenAllPublicFieldsAreStored()
        {
            //Given
            XmlContext saveContext = GetNewContext();
            //When
            var account = new BankAccount() { Number = "ACC1", Name = "Account1", Description = "DescAccount1" };
            var accountsRepository = saveContext.GetRepository<IRepository<BankAccount>>();
            accountsRepository.Add(account);

            var opType = new BankOperationType() { Name = "Type1" };
            var opTypeRepository = saveContext.GetRepository<IRepository<BankOperationType>>();
            opTypeRepository.Add(opType);

            var ops = new List<BankOperation>()
            {
                InitOperationWithTestData(1, account, opType, null),
                InitOperationWithTestData(2, account, opType, null),
                InitOperationWithTestData(3, account, opType, null)
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

            //Then - verify if operations can be loaded and have the same test data
            _saveMock.Setup(a => a.Load()).Returns(_savedContext);
            XmlContext loadContext = GetNewContext();
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
            var properties = GetPropertiesForAutoInitialization();

            for (int propertyNumber = 0; propertyNumber < properties.Length; propertyNumber++)
            {
                var property = properties[propertyNumber];
                var expectedValue = GetDefaultTestValueForProperty(property.PropertyType, propertyNumber + 1, operation.Id);
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

        private BankOperation InitOperationWithTestData(
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

            var properties = GetPropertiesForAutoInitialization();

            for (int propertyNumber = 0; propertyNumber < properties.Length; propertyNumber++)
            {
                InitPropertyWithTestData(properties[propertyNumber], operation, propertyNumber + 1, id);
            }

            return operation;
        }

        private static PropertyInfo[] GetPropertiesForAutoInitialization()
        {
            string[] skippedFields = new string[]
            {
                "Id",
                "BankAccount",
                "Type",
                "Card"
            };

            var properties = typeof(BankOperation).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(a => !skippedFields.Contains(a.Name))
                .OrderBy(b => b.Name).ToArray();
            return properties;
        }

        private void InitPropertyWithTestData(PropertyInfo propertyInfo, BankOperation operation, int propertyNumber, int operationNumber)
        {
            var valueToSet = GetDefaultTestValueForProperty(propertyInfo.PropertyType, propertyNumber, operationNumber);
            propertyInfo.SetValue(operation, valueToSet);
        }

        private static object GetDefaultTestValueForProperty(Type objectType, int propertyNumber, int operationNumber)
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
                toSet = operationNumber + 0.01m * propertyNumber;
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
