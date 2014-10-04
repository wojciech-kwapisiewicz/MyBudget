using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Core.InMemoryPersistance;
using MyBudget.Core.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.UnitTests.Persistance
{
    [TestFixture]
    public class BankOperationXmlRepositoryTests
    {
        [Test]
        public void GivenBankOperationXmlRepository_WhenElementAdded_ThenNextIdAssigned()
        {
            //Given
            var repo = new BankOperationXmlRepository(
                new Mock<IRepository<BankAccount, string>>().Object,
                new Mock<IRepository<BankOperationType, string>>().Object,
                new Mock<IRepository<BankStatement, int>>().Object);

            //When
            var bo1 = new BankOperation();
            var bo2 = new BankOperation();
            repo.Add(bo1);
            repo.Add(bo2);

            //Then
            Assert.AreEqual(1, bo1.Id);
            Assert.AreEqual(2, bo2.Id);
        }
    }
}
