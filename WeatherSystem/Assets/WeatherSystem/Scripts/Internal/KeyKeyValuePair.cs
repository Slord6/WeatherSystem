using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// An instance of the keys and value for a DoubleDictionary item
    /// </summary>
    /// <typeparam name="TPrimaryKey">The primary key</typeparam>
    /// <typeparam name="TSecondayKey">The secondary key</typeparam>
    /// <typeparam name="TValue">The lookup value</typeparam>
	public class KeyKeyValuePair<TPrimaryKey,TSecondayKey,TValue>
    {
        private TPrimaryKey primaryKey;
        private TSecondayKey secondaryKey;
        private TValue value;

        /// <summary>
        /// The primary key of this instance
        /// </summary>
        public TPrimaryKey PrimaryKey
        {
            get
            {
                return primaryKey;
            }
        }

        /// <summary>
        /// The secondary key of this instance
        /// </summary>
        public TSecondayKey SecondaryKey
        {
            get
            {
                return secondaryKey;
            }
        }

        /// <summary>
        /// The value of this instance
        /// </summary>
        public TValue Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Create a new key,key,value instance with given value
        /// </summary>
        /// <param name="primaryKey">The primary key of this instance</param>
        /// <param name="secondaryKey">The secondary key of this instance</param>
        /// <param name="value">The value of this instance</param>
        public KeyKeyValuePair(TPrimaryKey primaryKey, TSecondayKey secondaryKey, TValue value)
        {
            this.primaryKey = primaryKey;
            this.secondaryKey = secondaryKey;
            this.value = value;
        }

        /// <summary>
        /// The string representation of this KeyKeyValue pair
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return primaryKey.ToString() + "," + secondaryKey.ToString() + ": " + value.ToString();
        }
    }
}