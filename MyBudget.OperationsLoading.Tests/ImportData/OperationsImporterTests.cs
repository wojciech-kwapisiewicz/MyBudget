using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.ImportData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests.ImportData
{
    [TestFixture]
    public class OperationsImporterTests
    {
        private OperationsImporter _importer;
        private Mock<IRepository<BankOperation>> _operationsRepository;
        private Mock<IRepository<BankStatement>> _statementsRepository;
        private BankStatement _insertedStatement = null;
        private BankAccount _account = new BankAccount() { Name = "Aa", Number = "00" };
        private string _fileName = "fileNameToLoad";         

        [SetUp]
        public void SetUp()
        {
            _operationsRepository = new Mock<IRepository<BankOperation>>();
            _statementsRepository = new Mock<IRepository<BankStatement>>();
            _statementsRepository.Setup(par => par.Add(It.IsAny<BankStatement>()))
                .Callback<BankStatement>(st => _insertedStatement = st);
            _importer = new OperationsImporter(_operationsRepository.Object, _statementsRepository.Object);
        }

        [Test]
        public void ImportOperations_OperationsThatOverlap_NewStatementWithOnlyNewOperationsImported()
        {
            //Given
            var currentOperations = Enumerable.Range(1, 5)
                .Select(par => GetOperation(true, par.ToString(), par, par));
            var newOperations = Enumerable.Range(3, 5)
                .Select(par => GetOperation(true, par.ToString(), par));
            _operationsRepository.Setup(a => a.GetAll())
                .Returns(currentOperations);

            //When
            _importer.ImportOperations(_fileName, newOperations);

            //Then
            _statementsRepository.Verify(par => par.Add(It.IsAny<BankStatement>()), Times.Once);
            Assert.AreEqual(_fileName, _insertedStatement.FileName);
            Assert.AreEqual(3, _insertedStatement.Skipped);
            Assert.AreEqual(2, _insertedStatement.New);
            _operationsRepository.Verify(par => par.Add(It.IsAny<BankOperation>()), Times.Exactly(2));
        }

        [Test]
        public void ImportOperations_OperationsOverlapingUnclearedOperations_OperationsIgnored()
        {
            //Given
            var currentOperations = Enumerable.Range(1, 5)
                .Select(par => GetOperation(false, par.ToString(), par, par));
            var newOperations = Enumerable.Range(1, 5)
                .Select(par => GetOperation(false, par.ToString(), par));
            _operationsRepository.Setup(a => a.GetAll())
                .Returns(currentOperations);

            //When
            _importer.ImportOperations(_fileName, newOperations);

            //Then
            _operationsRepository.Verify(par => par.Add(It.IsAny<BankOperation>()), Times.Never);
            _statementsRepository.Verify(par => par.Add(It.IsAny<BankStatement>()), Times.Once);
            Assert.AreEqual(0, _insertedStatement.New);
            Assert.AreEqual(5, _insertedStatement.Skipped);
        }

        [Test]
        public void ImportOperations_ClearingOperationsForUnclearedOperations_OperationsAddedToNewStatementAndRemovedFromOldAndStatusClearedAdded()
        {
            //Given
            var currentOperations = Enumerable.Range(1, 5)
                .Select(par => GetOperation(false, par.ToString(), par, par));
            
            var currentStatement = new BankStatement();
            currentStatement.Operations.AddRange(currentOperations);
            _statementsRepository.Setup(par =>par.GetAll())
                .Returns(Enumerable.Repeat(currentStatement,1));

            var newOperations = Enumerable.Range(1, 3)
                .Select(par => GetOperation(true, par.ToString(), par));
            _operationsRepository.Setup(a => a.GetAll())
                .Returns(currentOperations);

            //When
            _importer.ImportOperations(_fileName, newOperations);

            //Then
            _statementsRepository.Verify(par => par.Add(It.IsAny<BankStatement>()), Times.Once);
            Assert.AreEqual(_fileName, _insertedStatement.FileName);
            Assert.AreEqual(3, _insertedStatement.Updated);
            Assert.AreEqual(3, currentStatement.Replaced);
            Assert.AreEqual(2, currentStatement.Operations.Count);
        }

        [Test]
        public void ImportOperations_ExistingOperationDescriptionMultilineAndLineFeedIsDifferent_OperationProperlyDiscoveredAsExisting()
        {
            List<BankOperation> currentOperations = new List<BankOperation>()
            {
                GetOperation(true, "Op1", description: "Op1\raaa"),
                GetOperation(true, "Op2", description: "Op2\r\naaa"),
                GetOperation(true, "Op3", description: "Op3aaa")
            };

            var currentStatement = new BankStatement();
            currentStatement.Operations.AddRange(currentOperations);
            _operationsRepository.Setup(a => a.GetAll())
                .Returns(currentOperations);

            List<BankOperation> newOperations = new List<BankOperation>()
            {
                GetOperation(true, "Op1", description: "Op1\r\naaa"),
                GetOperation(true, "Op2", description: "Op2\raaa"),
                GetOperation(true, "Op3", description: "Op3 aaa")
            };

            //When
            var importOperations = _importer.ImportOperations(_fileName, newOperations);

            //Then
            Assert.AreEqual(0, importOperations.Count());
            Assert.AreEqual(0, _insertedStatement.New);
            Assert.AreEqual(3, _insertedStatement.Skipped);
        }

        private BankOperation GetOperation(bool cleared, string title, decimal amount = 1, int id = 0, string description = null)
        {
            if(description==null)
            {
                description = title;
            }

            return new BankOperation()
            {
                Id = id,
                BankAccount = _account,
                Cleared = cleared,
                Title = title,
                Description = description,
                Amount = amount
            };
        }
    }
}
