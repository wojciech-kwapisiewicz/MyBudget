using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.DataContext
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T obj);
        void Delete(T obj);
    }

    public interface IRepository<T, TKey> : IRepository<T>
        where T : IIdentifiable<TKey>
    {
        T Get(TKey key);
    }
}
