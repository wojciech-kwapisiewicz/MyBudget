using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class Card : IIdentifiable<string>
    {
        public string Id
        {
            get { return CardNumber; }
        }

        public string CardNumber { get; set; }
        public string CardDescription { get; set; }
        public string CardType { get; set; }
    }
}
