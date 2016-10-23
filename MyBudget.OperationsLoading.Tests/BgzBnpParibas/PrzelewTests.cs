using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BgzBnpParibas;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests.BgzBnpParibas
{
    [TestFixture]
    public class PrzelewTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private Mock<IFillOperationFromDescriptionChain> fillMock;

        IFillOperationFromDescriptionChain przelew;
        IFillOperationFromDescriptionChain przelewPrzychodzacy;
        IFillOperationFromDescriptionChain przelewWychodzacy;

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.fillMock = new Mock<IFillOperationFromDescriptionChain>();
            this.przelew = new Przelew(
                new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object),
                fillMock.Object);
            this.przelewPrzychodzacy = new PrzelewPrzychodzacy(
                new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object),
                new ParseHelper(),
                fillMock.Object);
            this.przelewWychodzacy = new PrzelewWychodzacy(
                new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object),
                fillMock.Object);
        }

        [Test]
        public void GivenValidDescription_WhenParsed_OperationIsFilledWithData()
        {
            //Given
            string description = "PRZELEW NA RACHUNEK NUMER 12 3456 7890 1234 5678 9012 3456 Abc ";
            var operation = new BankOperation();

            //When
            this.przelew.Match(operation, description);

            //Then
            Assert.AreEqual("Abc", operation.Description);
            Assert.AreEqual("PRZELEW", operation.Type.Name);
            Assert.AreEqual("12345678901234567890123456", operation.CounterAccount);
        }

        [Test]
        public void GivenValidDescriptionIncomming_WhenParsed_OperationIsFilledWithData()
        {
            //Given
            string description = "PRZELEW UZNANIOWY (NADANO 01-10-2016) ABC WYPŁATA 01 10 2016    ABC. Z O.O.   UL.ABC 1 11-111 WARSZAWA  01 2345 6789 0123 4567 8901 2345 ABC CR/Aaaa ";
            var operation = new BankOperation();

            //When
            this.przelewPrzychodzacy.Match(operation, description);

            //Then
            Assert.AreEqual("ABC WYPŁATA 01 10 2016    ABC. Z O.O.   UL.ABC 1 11-111 WARSZAWA  01 2345 6789 0123 4567 8901 2345 ABC CR/Aaaa", operation.Description);
            Assert.AreEqual("PRZELEW PRZYCHODZĄCY", operation.Type.Name);
            Assert.AreEqual(new DateTime(2016, 10, 1), operation.OrderDate);
            Assert.AreEqual("01234567890123456789012345", operation.CounterAccount);
        }

        [Test]
        public void GivenValidDescriptionOutgoing_WhenParsed_OperationIsFilledWithData()
        {
            //Given
            string description = "PRZELEW OBCIĄŻENIOWY asadfasfdsaf   01 2345 6789 0123 4567 8901 2347 Abc 1 Asadfasdf ";
            var operation = new BankOperation();

            //When
            this.przelewWychodzacy.Match(operation, description);

            //Then
            Assert.AreEqual("asadfasfdsaf", operation.Description);
            Assert.AreEqual("PRZELEW DO INNEGO BANKU", operation.Type.Name);
            Assert.AreEqual("01234567890123456789012347", operation.CounterAccount);
        }
    }
}
