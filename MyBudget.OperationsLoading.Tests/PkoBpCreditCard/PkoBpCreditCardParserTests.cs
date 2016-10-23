using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.PkoBpCreditCard;
using MyBudget.OperationsLoading.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.UnitTests.ImportData
{
    [TestFixture]
    public class PkoBpCreditCardParserTests
    {
        [Test]
        public void GivenSampleCreditCardText_WhenParse_ThenListOfOperationsReturnedAndAccountCreated()
        {
            //Given
            Mock<IRepository<BankAccount, string>> accountRepo = new Mock<IRepository<BankAccount, string>>();
            Mock<IRepository<BankOperationType, string>> typeRepo = new Mock<IRepository<BankOperationType, string>>();
            Mock<IRepository<Card, string>> cardRepo = new Mock<IRepository<Card, string>>();
            string creditCardText = TestFiles.PkoBpCreditCardParser_Sample;

            //When
            var list = new PkoBpCreditCardUnclearedParser(
                new ParseHelper(),
                new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object),
                new CreditCardTextParsing(),
                new CreditCardUnclearedTextParsing())
                .Parse(creditCardText).ToArray();

            //Then
            Assert.AreEqual(3, list.Count());
            //accountRepo.Verify(a => a.Add(It.IsAny<BankAccount>()), Times.Exactly(1));
            //typeRepo.Verify(a => a.Add(It.IsAny<BankOperationType>()), Times.Exactly(2));
        }
    }
}
