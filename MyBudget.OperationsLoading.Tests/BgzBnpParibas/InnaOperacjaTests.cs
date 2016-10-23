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
    public class InnaOperacjaTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;

        IFillOperationFromDescriptionChain innaOperacja;

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.innaOperacja = new InnaOperacja(new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object));
        }

        [Test]
        public void GivenValidDescription_WhenParsed_OperationIsFilledWithData()
        {
            //Given
            string description = "           sdafasdfsa     ";
            var operation = new BankOperation();

            //When
            this.innaOperacja.Match(operation, description);

            //Then
            Assert.AreEqual("sdafasdfsa", operation.Description);
            Assert.AreEqual("INNA OPERACJA", operation.Type.Name);
        }
    }
}
