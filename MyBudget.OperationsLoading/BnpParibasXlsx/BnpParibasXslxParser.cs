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

        private IRepositoryHelper _repositoryHelper;
        private IOperationHandler _operationHandler;

        public BnpParibasXslxParser(IRepositoryHelper repositoryHelper, IOperationHandler operationHandler)
        {
            if (repositoryHelper == null)
                throw new ArgumentNullException("repositoryHelper");
            if(operationHandler==null)
                throw new ArgumentNullException("operationHandler");
            _repositoryHelper = repositoryHelper;
            _operationHandler = operationHandler;
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
                    var bankOperation = new BankOperation();
                    bankOperation.LpOnStatement = rowNum;
                    bankOperation.OrderDate = GetDateFromExcelRange(myWorksheet.Cells[rowNum, 1]);
                    bankOperation.ExecutionDate = GetDateFromExcelRange(myWorksheet.Cells[rowNum, 2]);
                    bankOperation.Amount = Convert.ToDecimal(myWorksheet.Cells[rowNum, 3].Value);
                    bankOperation.Description = myWorksheet.Cells[rowNum, 6].Value.ToString();
                    bankOperation.Type = _repositoryHelper.GetOrAddOperationType(myWorksheet.Cells[rowNum, 8].Value.ToString());
                    bankOperation.Cleared = true;

                    var bankAccountProduct = myWorksheet.Cells[rowNum, 7].Value.ToString();                    
                    var accountNumber = bankAccountProduct.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    bankOperation.BankAccount = _repositoryHelper.GetOrAddAccount(accountNumber);

                    //Parsing title and other details for different transactions
                    var counterpartyInfo = myWorksheet.Cells[rowNum, 5].Value.ToString();
                    _operationHandler.Handle(bankOperation, bankOperation.Description, myWorksheet.Cells[rowNum, 5].Value.ToString());

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
    }
}
