using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.XmlPersistance.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBudget.XmlPersistance.Tests
{
    [TestFixture]
    public class BasicXmlContextTests : XmlContextTestsBase
    {
        [Test]
        public void GivenXmlContext_WhenQueriedForProperRepo_ItIsReturned()
        {
            //Given
            var context = GetNewContext();

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
            var context = GetNewContext();

            //When
            var accountsRepository = context.GetRepository<IRepository<BankAccount>>();
            accountsRepository.Add(new BankAccount() { Number = "A", Name = "B", Description = "C" });

            Assert.IsTrue(context.SaveChanges());

            //Then             
            this._saveMock.Verify(a => a.Save(It.IsAny<XElement>()));
            Assert.IsNotNull(this._savedContext.ToString());
        }

        [Test]
        public void GivenXmlWithAccount_WhenContextCreated_ThenItContainsThisAccount()
        {
            //Given
            XElement el = XElement.Parse(TestFiles.input_saved1Account);
            _saveMock.Setup(a => a.Load()).Returns(el);

            //When

            //Then
            var repository = GetNewContext().GetRepository<IRepository<BankAccount>>();
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
            var context = GetNewContext();

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
            _saveMock.Verify(a => a.Save(It.IsAny<XElement>()));
            Assert.IsNotNull(_savedContext.ToString());

            _saveMock.Setup(a => a.Load()).Returns(new XElement(_savedContext));

            var context2 = new XmlContext(_saveMock.Object, GetRepoFactory());
        }

        [Test]
        public void GivenSomeContext_WhenNoChanges_ThenNoChangesReported()
        {
            //Given
            XElement el = XElement.Parse(TestFiles.savedEmptyContext);
            _saveMock.Setup(a => a.Load()).Returns(el);

            //Then
            Assert.IsFalse(GetNewContext().DataHasChanged());
        }

        [Test]
        public void GivenSomeContext_WhenThereAreChanges_ThenChangesReported()
        {
            //Given
            XElement el = XElement.Parse(TestFiles.savedEmptyContext);
            _saveMock.Setup(a => a.Load()).Returns(el);
            var context = GetNewContext();

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
            XElement el = XElement.Parse(TestFiles.savedEmptyContext);
            _saveMock.Setup(a => a.Load()).Returns(el);
            var context = GetNewContext();

            //When
            var accountRepo = context.GetRepository<IRepository<BankAccount>>();
            accountRepo.Add(new BankAccount() { Description = "a", Name = "b", Number = "c" });
            context.SaveChanges();

            //Then
            Assert.IsFalse(context.DataHasChanged());
        }
    }
}
