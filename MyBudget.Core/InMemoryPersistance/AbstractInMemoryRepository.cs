using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Core.InMemoryPersistance
{
    public abstract class AbstractInMemoryRepository<TObject, TKey> : IRepository<TObject, TKey>
        where TObject : IIdentifiable<TKey>
    {
        Dictionary<TKey, TObject> objects = new Dictionary<TKey, TObject>();
        
        protected int StoredElemets
        {
            get
            {
                return objects.Count;
            }
        }
        
        public TObject Get(TKey key)
        {
            TObject obj;
            objects.TryGetValue(key, out obj);
            return obj;
        }

        public IEnumerable<TObject> GetAll()
        {
            return objects.Values;
        }

        public virtual void Add(TObject obj)
        {
            objects.Add(obj.Id, obj);
        }

        public void Delete(TObject obj)
        {
            objects.Remove(obj.Id);
        }
    }
}
