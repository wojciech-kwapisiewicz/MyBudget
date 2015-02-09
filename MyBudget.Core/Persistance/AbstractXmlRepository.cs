using MyBudget.Core.DataContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MyBudget.Core.InMemoryPersistance
{
    public abstract class AbstractXmlRepository<TObject, TKey> : IRepository<TObject, TKey>
        where TObject : IIdentifiable<TKey>
    {
        protected Dictionary<TKey, TObject> storedObjects = new Dictionary<TKey, TObject>();

        protected TKey LastKey
        {
            get
            {
                return storedObjects.OrderBy(a => a.Key).Last().Key;
            }
        }

        private XmlSerializer serializer = new XmlSerializer(typeof(TObject[]));

        public virtual void Load(XElement element)
        {
            if (element == null)
                return;
            MemoryStream ms = new MemoryStream();
            element.Save(ms);
            ms.Position = 0;

            TObject[] loaded = serializer.Deserialize(ms) as TObject[];
            storedObjects = loaded.ToDictionary(a => a.Id);
        }

        public virtual XElement Save()
        {
            TObject[] toSave = storedObjects.Select(a => a.Value).ToArray();
            
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, toSave);
            ms.Position = 0;

            return XElement.Load(ms);
        }

        
        public TObject Get(TKey key)
        {
            TObject obj;
            storedObjects.TryGetValue(key, out obj);
            return obj;
        }

        public IEnumerable<TObject> GetAll()
        {
            return storedObjects.Values;
        }

        public virtual void Add(TObject obj)
        {
            storedObjects.Add(obj.Id, obj);
        }

        public void Delete(TObject obj)
        {
            storedObjects.Remove(obj.Id);
        }
    }
}
