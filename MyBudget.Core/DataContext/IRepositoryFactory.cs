using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.DataContext
{
    public interface IRepositoryFactory
    {
        T GetRepository<T>() where T : IRepository;
    }
}
