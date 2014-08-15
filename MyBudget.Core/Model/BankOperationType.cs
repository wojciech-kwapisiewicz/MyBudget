﻿using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.Model
{
    public class BankOperationType : IIdentifiable<string>
    {
        public string Name { get; set; }

        public string Id
        {
            get { return Name; }
        }
    }
}
