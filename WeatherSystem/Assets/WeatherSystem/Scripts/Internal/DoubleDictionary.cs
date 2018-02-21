using System.Collections.Generic;
using System;
using System.Collections;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// A generic dictionary requiring two keys to lookup values
    /// </summary>
    /// <typeparam name="TKey">The primary key type</typeparam>
    /// <typeparam name="TKeySecondary">The secondary key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    [Serializable]
    public class DoubleDictionary<TKey, TKeySecondary, TValue> : IEnumerable<KeyKeyValuePair<TKey, TKeySecondary, TValue>>
    {
        private Dictionary<TKey, Dictionary<TKeySecondary, TValue>> primaryDictionary;

        private int count;

        /// <summary>
        /// The number of elements stored in the DoubleDictionary
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// The number of primary keys stored in the DoubleDictionary
        /// </summary>
        public int PrimaryKeyCount
        {
            get
            {
                return primaryDictionary.Count;
            }
        }

        /// <summary>
        /// The number of secondary keys stored for a given primary key
        /// </summary>
        /// <param name="primaryKey">The primary key to count the secondary keys for</param>
        /// <returns>The count of keys if the primary key is in the dictionary, otherwise returns null</returns>
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

        /// <summary>
        /// Instantiate an empty DoubleDictionary
        /// </summary>
        public DoubleDictionary()
        {
            primaryDictionary = new Dictionary<TKey, Dictionary<TKeySecondary, TValue>>();
        }

        /// <summary>
        /// Access the value stored with the given keys
        /// </summary>
        /// <param name="primaryKey">The primary key</param>
        /// <param name="secondaryKey">The secondary key</param>
        /// <returns>The value stored with the given key pair</returns>
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

        /// <summary>
        /// Checks if the dictionary contains the given primary key
        /// </summary>
        /// <param name="primaryKey">The primary key to check</param>
        /// <returns>True if the DoubleDictionary has a primary key matching the given key, false otherwise</returns>
        public bool ContainsKey(TKey primaryKey)
        {
            return primaryDictionary.ContainsKey(primaryKey);
        }

        /// <summary>
        /// Checks if the dictionary has a value stored with the given key pair
        /// </summary>
        /// <param name="primaryKey">The primary key of the pair to check</param>
        /// <param name="secondaryKey">The secondary key of the pair to check</param>
        /// <returns>True if there is a value stored with the given keys, false otherwise</returns>
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

        /// <summary>
        /// Add a value with the given keys
        /// </summary>
        /// <param name="key">The primary key</param>
        /// <param name="secondaryKey">The secondary key</param>
        /// <param name="value">The value to store</param>
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

        /// <summary>
        /// Try to get a value with the given keys
        /// </summary>
        /// <param name="key">The primary key</param>
        /// <param name="secondaryKey">The secondary key</param>
        /// <param name="value">The found value, if one is found. otherwise value is null</param>
        /// <returns>True if a value was found, false otherwise</returns>
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

        /// <summary>
        /// Removes any primary keys that internally hold an empty secondary key dictionary
        /// </summary>
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

        /// <summary>
        /// Gets the iteration enumerator for each key,key,value set
        /// </summary>
        /// <returns>The current enumeration</returns>
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

        /// <summary>
        /// Get the iteration enumerator
        /// </summary>
        /// <returns>The iteration enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
