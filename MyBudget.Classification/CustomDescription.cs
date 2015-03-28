using MyBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBudget.Classification
{
    public class CustomDescription
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Card Card { get; set; }
    }
}
