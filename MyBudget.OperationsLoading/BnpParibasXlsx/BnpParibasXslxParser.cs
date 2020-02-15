using MyBudget.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.BnpParibasXlsx
{
    public class BnpParibasXslxParser : IParser
    {
        public string Name => Resources.BnpParibasXslxName;

        public string SupportedFileExtensions => Resources.BnpParibasXslxFilter;

        ParseHelper _parseHelper;
        IRepositoryHelper _repositoryHelper;

        public BnpParibasXslxParser(ParseHelper parseHelper, IRepositoryHelper repositoryHelper)
        {
            if (parseHelper == null)
                throw new ArgumentNullException("parseHelper");
            _parseHelper = parseHelper;
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            _repositoryHelper = repositoryHelper;
        }

        public IEnumerable<BankOperation> Parse(string inputString)
        {
            return Parse(ToStream(inputString));
        }

        public Stream ToStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public IEnumerable<BankOperation> Parse(Stream stream)
        {
            List<BankOperation> ops = new List<BankOperation>();
            using (ExcelPackage xlPackage = new ExcelPackage(stream))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 2; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    var orderDate = GetDateFromExcelRange(myWorksheet.Cells[rowNum, 1]);
                    var executionDate = GetDateFromExcelRange(myWorksheet.Cells[rowNum, 2]);
                    var amount = Convert.ToDecimal(myWorksheet.Cells[rowNum, 3].Value);  
                    
                    var counterAccount = myWorksheet.Cells[rowNum, 5].Value.ToString();
                    var description = myWorksheet.Cells[rowNum, 6].Value.ToString();                    
                    var typeName = myWorksheet.Cells[rowNum, 8].Value.ToString();
                    var product = myWorksheet.Cells[rowNum, 7].Value.ToString();

                    var accountNumber = product.Split('\n')[1];

                    //Parsing title for different transactions
                    string title = string.Empty;
                    switch (typeName)
                    {
                        case "Transakcja BLIK":
                            title = GetBlikTitle(description);
                            break;
                        case "Transakcja kartą":
                            title = GetCardDetails(description);
                            break;
                        default:
                            title = _parseHelper.GetFirstNCharacters(description, 30);
                            break;
                    };

                    BankAccount account = _repositoryHelper.GetOrAddAccount(accountNumber);

                    var bankOperation = new BankOperation()
                    {
                        LpOnStatement = rowNum,
                        BankAccount = account,
                        OrderDate = orderDate,
                        ExecutionDate = executionDate,
                        Amount = amount,
                        Description = description,
                        Title = title,
                        CounterAccount = counterAccount,

                        Type = _repositoryHelper.GetOrAddOperationType(typeName),
                        Cleared = true
                    };

                    ops.Add(bankOperation);
                }
            }

            return ops;
        }

        public DateTime GetDateFromExcelRange(ExcelRange excelRange)
        {
            double dateValue = (double) excelRange.Value;
            DateTime date = DateTime.FromOADate(dateValue);
            return date;
        }

        public string GetBlikTitle(string description)
        {
            var transactionDetails = description.Replace("Transakcja BLIK, ", "");
            return _parseHelper.GetFirstNCharacters(transactionDetails, 30);
        }

        public string GetCardDetails(string description)
        {
            string regexPattern = @"^(\d*------\d*) (\w* \w*) (.*)$";
            var matchedParts = Regex.Match(description, regexPattern);

            var cardNum = matchedParts.Groups[1].Value;
            var cardHolder = matchedParts.Groups[2].Value;
            var transactionDetails = matchedParts.Groups[3].Value;

            return _parseHelper.GetFirstNCharacters(transactionDetails, 30);
        }
    }
}
