using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BgzBnpParibasCsv;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests.BgzBnpParibasCsv
{
    [TestFixture]
    public class WyplataBankomatTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private Mock<IFillOperationFromDescriptionChain> fillMock;

        WyplataBankomat parser;

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.fillMock = new Mock<IFillOperationFromDescriptionChain>();                
               this.parser = new WyplataBankomat(
                   new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object),
                   new ParseHelper(),
                   fillMock.Object);
        }

        [Test]
        public void GivenValidDescription_WhenParsed_OperationIsFilledWithDataAndCardIsAdded()
        {
            //Given
            string description = "OPERACJA KARTĄ ZLOTA 123456XXXXXX7890 000001 WYPL ATA GOTÓWKI A111 BANK1 SA   10.11PLN D=04.10.2016  ";
            var operation = new BankOperation();

            //When
            this.parser.Match(operation, description);

            //Then
            Assert.AreEqual("WYPŁATA KARTĄ Z BANKOMATU A111 BANK1 SA", operation.Description);
            Assert.AreEqual("WYPŁATA KARTĄ Z BANKOMATU", operation.Type.Name);
            Assert.AreEqual(new DateTime(2016, 10, 4), operation.OrderDate);
            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == "123456XXXXXX7890")));
            Assert.AreEqual("123456XXXXXX7890", operation.Card.CardNumber);
        }
    }
}
