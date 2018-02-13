using System.Collections.Generic;
using System;
using System.Collections;

namespace WeatherSystem.Internal
{
    [Serializable]
    public class DoubleDictionary<TKey, TKeySecondary, TValue> : IEnumerable<KeyKeyValuePair<TKey, TKeySecondary, TValue>>
    {
        private Dictionary<TKey, Dictionary<TKeySecondary, TValue>> primaryDictionary;

        private int count;

        public int Count
        {
            get
            {
                return count;
            }
        }

        public int PrimaryKeyCount
        {
            get
            {
                return primaryDictionary.Count;
            }
        }

        public int? SecondaryKeyCount(TKey primaryKey)
        {
            if (primaryDictionary.ContainsKey(primaryKey))
            {
                return primaryDictionary[primaryKey].Count;
            }
            else
            {
                return null;
            }
        }

        public DoubleDictionary()
        {
            primaryDictionary = new Dictionary<TKey, Dictionary<TKeySecondary, TValue>>();
        }

        public TValue this[TKey primaryKey, TKeySecondary secondaryKey]
        {
            get
            {
                return primaryDictionary[primaryKey][secondaryKey];
            }
            set
            {
                primaryDictionary[primaryKey][secondaryKey] = value;
            }
        }

        public bool ContainsKey(TKey primaryKey)
        {
            return primaryDictionary.ContainsKey(primaryKey);
        }

        public bool ContainsKey(TKey primaryKey, TKeySecondary secondaryKey)
        {
            if (ContainsKey(primaryKey))
            {
                return primaryDictionary[primaryKey].ContainsKey(secondaryKey);
            }
            else
            {
                return false;
            }
        }

        public void Add(TKey key, TKeySecondary secondaryKey, TValue value)
        {
            Dictionary<TKeySecondary, TValue> secondaryDictionary;
            if (primaryDictionary.ContainsKey(key))
            {
                secondaryDictionary = primaryDictionary[key];
                secondaryDictionary.Add(secondaryKey, value);
            }
            else
            {
                secondaryDictionary = new Dictionary<TKeySecondary, TValue>();
                secondaryDictionary.Add(secondaryKey, value);
                primaryDictionary.Add(key, secondaryDictionary);
            }
            count++;

        }

        public bool TryGetValue(TKey key, TKeySecondary secondaryKey, out TValue value)
        {
            Dictionary<TKeySecondary, TValue> secondaryDictionary;
            bool got = primaryDictionary.TryGetValue(key, out secondaryDictionary);

            if (got)
            {
                got = secondaryDictionary.TryGetValue(secondaryKey, out value);
            }
            else
            {
                value = default(TValue);
            }
            return got;
        }

        public void Remove(TKey key, TKeySecondary secondaryKey)
        {
            TValue value;
            bool exists = TryGetValue(key, secondaryKey, out value);

            if (exists)
            {
                primaryDictionary[key].Remove(secondaryKey);
                count--;
            }
        }

        public void CleanEmpty()
        {
            List<TKey> keysToClear = new List<TKey>();
            foreach (KeyValuePair<TKey, Dictionary<TKeySecondary, TValue>> embeddedPair in primaryDictionary)
            {
                if (embeddedPair.Value.Count == 0)
                {
                    keysToClear.Add(embeddedPair.Key);
                }
            }

            foreach (TKey primaryKey in keysToClear)
            {
                primaryDictionary.Remove(primaryKey);
            }
        }

        public IEnumerator<KeyKeyValuePair<TKey, TKeySecondary, TValue>> GetEnumerator()
        {
            //iterate over the primary keys
            foreach(KeyValuePair<TKey, Dictionary<TKeySecondary,TValue>> outerPair in primaryDictionary)
            {
                //iterate over the inner dictionary of the primary key
                foreach(KeyValuePair<TKeySecondary,TValue> innerPair in outerPair.Value)
                {
                    //yield the primary key, secondary key for this dictionary and the associated value
                    yield return new KeyKeyValuePair<TKey, TKeySecondary, TValue>(outerPair.Key, innerPair.Key, innerPair.Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
