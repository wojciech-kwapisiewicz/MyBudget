using Moq;
using MyBudget.Core.DataContext;
using MyBudget.Model;
using MyBudget.OperationsLoading.BnpParibasXlsx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBudget.OperationsLoading.Tests.BnpParibasXlsx
{
    [TestFixture("v1")]
    [TestFixture("v2")]
    public class BnpParibasXslxParserTests
    {
        private Mock<IRepository<BankAccount, string>> accountRepo;
        private Mock<IRepository<BankOperationType, string>> typeRepo;
        private Mock<IRepository<Card, string>> cardRepo;
        private RepositoryHelper repositoryHelper;
        private BnpParibasXslxParser parser;
        private ParseHelper parseHelper = new ParseHelper();

        private List<BankAccount> mockAccountsCreated = new List<BankAccount>();
        private Stream XlsxFile = null;
        Dictionary<string, Stream> BNPFileVersions = new Dictionary<string, Stream>()
        {
            {"v1", Resources.TestFiles.BNP_TestOperations_v1.ToStream() },
            {"v2", Resources.TestFiles.BNP_TestOperations_v2.ToStream() }
        };

        public BnpParibasXslxParserTests(string version)
        {
            XlsxFile = BNPFileVersions[version];
        }

        [SetUp]
        public void SetUp()
        {
            this.accountRepo = new Mock<IRepository<BankAccount, string>>();
            this.typeRepo = new Mock<IRepository<BankOperationType, string>>();
            this.cardRepo = new Mock<IRepository<Card, string>>();
            this.repositoryHelper = new RepositoryHelper(accountRepo.Object, typeRepo.Object, cardRepo.Object);
            this.parser = new BnpParibasXslxParser(repositoryHelper,
                new BlikHandler(parseHelper, 
                new CardHandler(parseHelper, repositoryHelper, 
                new DefaultHandler(parseHelper))));

            this.accountRepo.Setup(a => a.Get(It.IsAny<string>())).Returns<string>(
                a => mockAccountsCreated.FirstOrDefault(x => x.Number == a));
            this.accountRepo.Setup(a => a.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(
                a => mockAccountsCreated.Add(a));
        }

        [Test]
        [SetUICulture("pl")]
        public void BgzParser_FilterCSVFilesPL()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("BNPParibas (XLS / Excel) (.xlsx)|*.xlsx", extensions);
        }

        [Test]
        [SetUICulture("en")]
        public void BgzParser_FilterCSVFilesEN()
        {
            //When            
            var extensions = this.parser.SupportedFileExtensions;

            //Then
            Assert.AreEqual("BNPParibas operations (XLS / Excel) (.xlsx)|*.xlsx", extensions);
        }

        [Test]
        public void GivenBasicCases_WhenParseBnpFormat_Then10OperationsAreReturned_AccountAnd8NewTypesAreAdded()
        {
            //When                
            var operations = parser.Parse(XlsxFile);

            //Then
            Assert.AreEqual(10, operations.Count());
            VerifyOperations(operations);
            VerifyAccountAndCardNumbersAndOperationTypes();
        }

        private static void VerifyOperations(IEnumerable<BankOperation> operations)
        {
            BankOperation checkOp;

            #region Operation type - default

            //1
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 17) &&
                a.Amount == -1000.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 17));
            Assert.AreEqual(checkOp.Type.Name, "Operacja kredytowa");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Operacja kredytowa nr 01234568");
            Assert.AreEqual(checkOp.Title, "Operacja kredytowa nr 01234568".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, "PL11222233334444555566667777");
            Assert.AreEqual(checkOp.CounterParty, "Kowalska A i Kowalski J" + Environment.NewLine +
                "Nowa 11/1" + Environment.NewLine +
                "Warszawa" + Environment.NewLine +
                "POLSKA");

            //2
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 17) &&
                a.Amount == -1200.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 17));
            Assert.AreEqual(checkOp.Type.Name, "Prowizje i opłaty");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Prowizja kredytowa nr 01234567");
            Assert.AreEqual(checkOp.Title, "Prowizja kredytowa nr 01234567".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, "PL11222233334444555566667777");
            Assert.AreEqual(checkOp.CounterParty, "Kowalska A i Kowalski J" + Environment.NewLine +
                "Nowa 11/1" + Environment.NewLine +
                "Warszawa" + Environment.NewLine +
                "POLSKA");

            #endregion

            #region Operation type - bank transfer

            //3
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 21) &&
                a.Amount == 2345.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 21));
            Assert.AreEqual(checkOp.Type.Name, "Przelew przychodzący");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Przelew przychodzacy - wyplata");
            Assert.AreEqual(checkOp.Title, "Przelew przychodzacy - wyplata".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, TestBankData.ExternalAccount_TestAccount1.Compact());
            Assert.AreEqual(checkOp.CounterParty, "Nadawca przelewu" + Environment.NewLine +
                "00-000 Warszawa, ul. Nowa 11/3" + Environment.NewLine +
                "PL");

            //4
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 20) &&
                a.Amount == -300.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 20));
            Assert.AreEqual(checkOp.Type.Name, "Przelew wychodzący");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Oplata za cos");
            Assert.AreEqual(checkOp.Title, "Oplata za cos".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, TestBankData.ExternalAccount_TestAccount1.Compact());
            Assert.AreEqual(checkOp.CounterParty, "Odbiorca przelewu" + Environment.NewLine +
                "00-000 Warszawa, ul. Nowa 11/3");

            //5

            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 19) &&
                a.Amount == -150.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 19));
            Assert.AreEqual(checkOp.Type.Name, "Zlecenie stałe");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Oplata stala za prad");
            Assert.AreEqual(checkOp.Title, "Oplata stala za prad".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, TestBankData.ExternalAccount_TestAccount2.Compact());
            Assert.AreEqual(checkOp.CounterParty, "Dostawca pradu" + Environment.NewLine +
                "Nowa 11/2" + Environment.NewLine +
                "00-000 Warszawa");

            //6
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 10) &&
                a.Amount == -120.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 10));
            Assert.AreEqual(checkOp.Type.Name, "Przelew internetowy");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Numer 12345678901234567890");
            Assert.AreEqual(checkOp.Title, "Numer 12345678901234567890".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, TestBankData.ExternalAccount_TestAccount1.Compact());
            Assert.AreEqual(checkOp.CounterParty, "Sklep");

            #endregion

            #region Operation type - blik

            //7
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 16) &&
                a.Amount == 50.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 16));
            Assert.AreEqual(checkOp.Type.Name, "Transakcja BLIK");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Transakcja BLIK, Zwrot BLIK, Zwrot BLIK internet, Nr 12345678901, Zwrot za zakupy");
            Assert.AreEqual(checkOp.Title, "Zwrot BLIK, Zwrot BLIK internet, Nr 12345678901, Zwrot za zakupy".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, TestBankData.ExternalAccount_TestAccount1.Compact());
            Assert.AreEqual(checkOp.CounterParty, "Sklep" + Environment.NewLine +
                "PL");

            //8
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 11) &&
                a.Amount == -300.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 11));
            Assert.AreEqual(checkOp.Type.Name, "Transakcja BLIK");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "Transakcja BLIK, 12345678901, Platnosc za zakupy");
            Assert.AreEqual(checkOp.Title, "12345678901, Platnosc za zakupy".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card, null);
            Assert.AreEqual(checkOp.CounterAccount, TestBankData.ExternalAccount_TestAccount1.Compact());
            Assert.AreEqual(checkOp.CounterParty, "Sklep" + Environment.NewLine +
                "PL");

            #endregion

            #region Operation type - card transaction

            //9
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 25) &&
                a.Amount == -2.40M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 25));
            Assert.AreEqual(checkOp.Type.Name, "Transakcja kartą");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "512345------0010 ANNA KOWALSKA WARSZAWA MPK1234 1WRO URBANCARD POL 2,40 PLN 2020-02-25");
            Assert.AreEqual(checkOp.Title, "WARSZAWA MPK1234 1WRO URBANCARD POL 2,40 PLN 2020-02-25".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card.CardNumber, TestBankData.CardBNP_No1);
            Assert.AreEqual(checkOp.CounterAccount, null);
            Assert.AreEqual(checkOp.CounterParty, null);

            //10
            checkOp = operations.FirstOrDefault(a =>
                a.OrderDate == new DateTime(2020, 02, 23) &&
                a.Amount == -234.00M);

            Assert.IsNotNull(checkOp);
            Assert.AreEqual(checkOp.BankAccount.Number, TestBankData.BNPParibas_TestAccount1.Compact());
            Assert.AreEqual(checkOp.ExecutionDate, new DateTime(2020, 02, 24));
            Assert.AreEqual(checkOp.Type.Name, "Transakcja kartą");
            Assert.AreEqual(checkOp.Cleared, true);
            Assert.AreEqual(checkOp.Description, "512345------0020 JAN KOWALSKI WARSZAWA SKLEP WARSZAWA POL 234,00 PLN 2020-02-24");
            Assert.AreEqual(checkOp.Title, "WARSZAWA SKLEP WARSZAWA POL 234,00 PLN 2020-02-24".GetFirstNCharacters(OperationsLoadingConsts.OperationTitleLength));
            Assert.AreEqual(checkOp.EndingBalance, 0.0M);
            Assert.AreEqual(checkOp.Card.CardNumber, TestBankData.CardBNP_No2);
            Assert.AreEqual(checkOp.CounterAccount, null);
            Assert.AreEqual(checkOp.CounterParty, null);
            #endregion
        }

        private void VerifyAccountAndCardNumbersAndOperationTypes()
        {
            //We want just 1 account to be inserted
            accountRepo.Verify(repo => repo.Add(
                It.Is<BankAccount>(account =>
                account.Number == TestBankData.BNPParibas_TestAccount1.Compact())), Times.Once);

            accountRepo.Verify(repo => repo.Get(
                It.Is<string>(accountNumber => accountNumber == TestBankData.BNPParibas_TestAccount1.Compact())), Times.Exactly(10));

            //Likewise this should be changed to adjust
            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == TestBankData.CardBNP_No1)));

            cardRepo.Verify(repo => repo.Add(
                It.Is<Card>(card =>
                card.CardNumber == TestBankData.CardBNP_No2)));

            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Transakcja kartą")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Przelew przychodzący")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Przelew wychodzący")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Zlecenie stałe")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Operacja kredytowa")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Prowizje i opłaty")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Transakcja BLIK")));
            typeRepo.Verify(repo => repo.Add(
                It.Is<BankOperationType>(type =>
                type.Name == "Przelew internetowy")));
        }
    }
}
