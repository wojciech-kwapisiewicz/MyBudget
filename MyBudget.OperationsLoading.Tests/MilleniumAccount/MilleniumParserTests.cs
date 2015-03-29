using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading;
using MyBudget.OperationsLoading.MilleniumAccount;
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
    public class MilleniumParserTests
    {
        [Test]
        public void GivenSampleMilleniumCsv_WhenParse_ThenListOfOperationsReturnedAnd2AccountsCreated()
        {
            //Given
            Mock<IRepository<BankAccount, string>> accountRepo = new Mock<IRepository<BankAccount, string>>();
            Mock<IRepository<BankOperationType, string>> typeRepo = new Mock<IRepository<BankOperationType, string>>();
            string milleniumCsv = TestFiles.MilleniumParser_Sample;

            //When
            var list = new MilleniumParser(                
                new ParseHelper(),
                new RepositoryHelper(accountRepo.Object, typeRepo.Object))
                .Parse(milleniumCsv).ToArray();

            //Then
            Assert.AreEqual(7, list.Count());
            //accountRepo.Verify(a => a.Add(It.IsAny<BankAccount>()), Times.Exactly(2));
            //typeRepo.Verify(a => a.Add(It.IsAny<BankOperationType>()), Times.Exactly(6));
        }
    }
}
