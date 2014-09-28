using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBudget.Core.DataContext
{
    public interface IRepository
    {
        void Load(XElement element);
        XElement Save();
    }

    public interface IRepository<T> : IRepository
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
