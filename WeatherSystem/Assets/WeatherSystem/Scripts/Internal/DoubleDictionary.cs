using System.Collections.Generic;

namespace WeatherSystem.Internal
{
    public class DoubleDictionary<TKey, TKeySecondary, TValue>
    {
        private Dictionary<TKey, Dictionary<TKeySecondary, TValue>> primaryDictionary;

        public DoubleDictionary()
        {
            primaryDictionary = new Dictionary<TKey, Dictionary<TKeySecondary, TValue>>();
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

    }
}
