using MyBudget.Core.DataContext;
using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MyBudget.XmlPersistance
{
    public class BankOperationXmlRepository : AbstractXmlRepository<BankOperation, int>
    {
        private IRepository<BankAccount, string> _bankAccounts;
        private IRepository<BankOperationType, string> _operationTypes;
        private IRepository<BankStatement, int> _statements;

        public BankOperationXmlRepository(
            IRepository<BankAccount, string> bankAccounts,
            IRepository<BankOperationType, string> operationTypes,
            IRepository<BankStatement, int> statements)
        {
            if (bankAccounts == null)
                throw new ArgumentNullException("bankAccounts");
            if (operationTypes == null)
                throw new ArgumentNullException("operationTypes");
            if (statements == null)
                throw new ArgumentNullException("statements");

            _bankAccounts = bankAccounts;
            _operationTypes = operationTypes;
            _statements = statements;
        }

        [XmlType("BankOperation")]
        public class BankOperationSaveModel
        {
            public int Id { get; set; }

            public string BankAccountId { get; set; }
            public string TypeId { get; set; }
            public int StatementId { get; set; }

            public int LpOnStatement { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime ExecutionDate { get; set; }
            public decimal Amount { get; set; }
            public decimal EndingBalance { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public string Category { get; set; }
            public string SubCategory { get; set; }
            public Card Card { get; set; }


            public static BankOperationSaveModel FromOperation(
                BankOperation bo, 
                BankStatement statement,
                BankAccount account,
                BankOperationType type
                )
            {
                var sbo = new BankOperationSaveModel()
                {
                    Id = bo.Id,
                    LpOnStatement = bo.LpOnStatement,
                    OrderDate = bo.OrderDate,
                    ExecutionDate = bo.ExecutionDate,
                    Amount = bo.Amount,
                    EndingBalance = bo.EndingBalance,
                    Title = bo.Title,
                    Description = bo.Description,
                    Category = bo.Category,
                    SubCategory = bo.SubCategory
                };

                sbo.StatementId = statement.Id;
                sbo.BankAccountId = account.Id;
                sbo.TypeId = type.Id;

                return sbo;
            }

            public BankOperation ToOperation(BankOperationXmlRepository rp)
            {
                var bo = new BankOperation()
                {
                    Id = this.Id,
                    LpOnStatement = this.LpOnStatement,
                    OrderDate = this.OrderDate,
                    ExecutionDate = this.ExecutionDate,
                    Amount = this.Amount,
                    EndingBalance = this.EndingBalance,
                    Title = this.Title,
                    Description = this.Description,
                    Category = this.Category,
                    SubCategory = this.SubCategory
                };

                bo.Type = rp._operationTypes.Get(this.TypeId);
                bo.BankAccount = rp._bankAccounts.Get(this.BankAccountId);
                return bo;
            }
        }

        public override void Add(BankOperation obj)
        {
            obj.Id = LastKey + 1;
            base.Add(obj);
        }

        private XmlSerializer serializer = new XmlSerializer(typeof(BankOperationSaveModel[]));

        public override System.Xml.Linq.XElement Save()
        {
            var statements = _statements.GetAll();
            var operations = storedObjects.Select(a => a.Value);

            BankOperationSaveModel[] toSave = storedObjects.Select(a =>
                BankOperationSaveModel.FromOperation(
                a.Value,
                statements.FirstOrDefault(x => x.Operations.Any(b => b.Id == a.Value.Id)),
                a.Value.BankAccount, a.Value.Type)).ToArray();

            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, toSave);
            ms.Position = 0;

            return XElement.Load(ms);
        }

        public override void Load(System.Xml.Linq.XElement element)
        {
            if (element == null)
                return;
            MemoryStream ms = new MemoryStream();
            element.Save(ms);
            ms.Position = 0;

            BankOperationSaveModel[] loaded = serializer.Deserialize(ms) as BankOperationSaveModel[];

            List<BankOperation> allOperations = new List<BankOperation>(loaded.Length);
            foreach (var groupedByStatement in loaded.GroupBy(a=>a.StatementId))
            {
                var operationsForStatement = groupedByStatement
                    .Select(ops => ops.ToOperation(this));
                _statements.Get(groupedByStatement.Key).Operations.AddRange(operationsForStatement);
                allOperations.AddRange(operationsForStatement);
            }

            storedObjects = allOperations.ToDictionary(a => a.Id);
        }
    }
}
