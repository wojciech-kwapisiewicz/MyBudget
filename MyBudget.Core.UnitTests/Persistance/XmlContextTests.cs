using MyBudget.Core.DataContext;
using MyBudget.Core.InMemoryPersistance;
using MyBudget.Core.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBudget.Core.UnitTests.InMemoryPersistance
{
    [TestFixture]
    public class XmlContextTests
    {
        [Test]
        public void GivenEmptyContext_WhenBankAccountAddedAndSaved_ThenXElementContainsAccount()
        {
            //Given
            var context = new XmlContext(new XElement("root"));
            
            //When
            var accountsRepository = context.GetRepository<IRepository<BankAccount>>();
            accountsRepository.Add(new BankAccount() { Number = "A", Name = "B", Description = "C" });
            
            Assert.IsTrue(context.SaveChanges());

            //Then

        }

        [Test]
        public void GivenXmlWithAccount_WhenContextCreated_ThenItContainsThisAccount()
        {
            //Given
            string xmlToLoad = ManifestStreamReaderHelper
                .ReadEmbeddedResource(typeof(XmlContextTests), "saved1Account.xml");
            XElement el = XElement.Parse(xmlToLoad);

            //When
            var context = new XmlContext(el);

            //Then
            var repository = context.GetRepository<IRepository<BankAccount>>();
            Assert.IsTrue(repository.GetAll().Any(a =>
                a.Name == "AccountName1" &&
                a.Number == "AccountNumber2" &&
                a.Description == "AccountDescription3" &&
                a.Id == "AccountNumber2"));
        }
    }
}
