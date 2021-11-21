using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Classification.Tests
{
    [TestFixture]
    public class GenericClassificationRuleTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private ClassificationDefinition classificationDefinition;
        private List<BankOperation> bankOperationsToBeMatched;
        private List<BankOperation> bankOperationsToBeSkipped;

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.accountRepo.Setup(a => a.GetAll()).Returns<IEnumerable<BankAccount>>(
                a => new List<BankAccount>());
            //a => mockAccountsCreated.FirstOrDefault(x => x.Number == a));
            this.classificationDefinition = new ClassificationDefinition()
            {
                Category = "Category_ToAssign",
                SubCategory = "SubCategory_ToAssign",
                Description = "Description_ToAssign"
            };

            this.bankOperationsToBeMatched = new List<BankOperation>();
            this.bankOperationsToBeSkipped = new List<BankOperation>();
        }

        [Test]
        public void GivenNonRegexRule_WhenClassificationTriggered_ThenFieldsAreLookedUpPlain()
        {
            //Given
            var testedRule = new GenericClassificationRule(this.classificationDefinition, this.accountRepo.Object);
            this.classificationDefinition.Rules.Add(new ClassificationRule() { IsRegularExpression = false, SearchedPhrase = "[A-Z]" });

            this.bankOperationsToBeMatched.Add(new BankOperation() { Description = "[A-Z]" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Description = "XXX" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Description = "123456" });

            //When
            foreach (var testedBankOperation in this.bankOperationsToBeMatched)
            {
                //Then
                var result = testedRule.DoMatch(testedBankOperation);
                Assert.IsTrue(result);
            }

            //When
            foreach (var testedBankOperation in this.bankOperationsToBeSkipped)
            {
                //Then
                var result = testedRule.DoMatch(testedBankOperation);
                Assert.IsFalse(result);
            }
        }

        [Test]
        public void GivenRegexRule_WhenClassificationTriggered_ThenFieldsAreLookedUpByRegex()
        {
            //Given
            var testedRule = new GenericClassificationRule(this.classificationDefinition, this.accountRepo.Object);
            this.classificationDefinition.Rules.Add(new ClassificationRule() { IsRegularExpression = true, SearchedPhrase = "[A-Z]" });


            this.bankOperationsToBeMatched.Add(new BankOperation() { Description = "[A-Z]" });
            this.bankOperationsToBeMatched.Add(new BankOperation() { Description = "XXX" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Description = "123456" });

            //When
            foreach (var testedBankOperation in this.bankOperationsToBeMatched)
            {
                //Then
                var result = testedRule.DoMatch(testedBankOperation);
                Assert.IsTrue(result);
            }

            //When
            foreach (var testedBankOperation in this.bankOperationsToBeSkipped)
            {
                //Then
                var result = testedRule.DoMatch(testedBankOperation);
                Assert.IsFalse(result);
            }
        }

        [Test]
        public void GivenNonRegexRule_WhenClassificationTriggered_ThenRelevantFieldsAreLookedUpPlain()
        {
            //Fields that are relevant:  "Description", "Title", "CounterParty" 

        //Given
        var testedRule = new GenericClassificationRule(this.classificationDefinition, this.accountRepo.Object);
            this.classificationDefinition.Rules.Add(new ClassificationRule() { IsRegularExpression = false, SearchedPhrase = "[A-Z]" });

            this.bankOperationsToBeMatched.Add(new BankOperation() { Description = "[A-Z]" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Description = "XXX" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Description = "123456" });
            this.bankOperationsToBeMatched.Add(new BankOperation() { Title = "[A-Z]" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Title = "XXX" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { Title = "123456" });
            this.bankOperationsToBeMatched.Add(new BankOperation() { CounterParty = "[A-Z]" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { CounterParty = "XXX" });
            this.bankOperationsToBeSkipped.Add(new BankOperation() { CounterParty = "123456" });

            //When
            foreach (var testedBankOperation in this.bankOperationsToBeMatched)
            {
                //Then
                var result = testedRule.DoMatch(testedBankOperation);
                Assert.IsTrue(result);
            }

            //When
            foreach (var testedBankOperation in this.bankOperationsToBeSkipped)
            {
                //Then
                var result = testedRule.DoMatch(testedBankOperation);
                Assert.IsFalse(result);
            }
        }

        [Test]
        public void GivenRuleWithAccount_WhenClassificationTriggered_ThenAccountRelevantOperationsAreFound()
        {
            Assert.Inconclusive("TBD - technical debt to be cleaned up.");
        }
    }
}
