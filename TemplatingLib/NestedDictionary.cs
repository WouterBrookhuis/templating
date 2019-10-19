using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class NestedDictionary<K, V> : IDictionary<K, V>
    {
        private Dictionary<K, V> _self;
        private Dictionary<K, V> _parent;

        public NestedDictionary(Dictionary<K, V> parent)
        {
            _self = new Dictionary<K, V>();
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public V this[K key]
        {
            get => _self[key];
            set => _self[key] = value;
        }

        public ICollection<K> Keys => _self.Keys;

        public ICollection<V> Values => _self.Values;

        public int Count => _self.Count;

        public bool IsReadOnly => false;

        public void Add(K key, V value)
        {
            _self.Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            _self.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _self.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            if (_self.Contains(item))
            {
                return true;
            }
            if (_parent != null)
            {
                return _parent.Contains(item);
            }
            return false;
        }

        public bool ContainsKey(K key)
        {
            if(_self.ContainsKey(key))
            {
                return true;
            }
            if(_parent != null)
            {
                return _parent.ContainsKey(key);
            }
            return false;
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _self.GetEnumerator();
        }

        public bool Remove(K key)
        {
            return _self.Remove(key);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(K key, out V value)
        {
            if (_self.TryGetValue(key, out value))
            {
                return _self.TryGetValue(key, out value);
            }
            if (_parent != null)
            {
                return _parent.TryGetValue(key, out value);
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
