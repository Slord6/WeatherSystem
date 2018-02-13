using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WeatherSystem.Internal
{
	public class KeyKeyValuePair<TPrimaryKey,TSecondayKey,TValue>
    {
        private TPrimaryKey primaryKey;
        private TSecondayKey secondaryKey;
        private TValue value;

        public TPrimaryKey PrimaryKey
        {
            get
            {
                return primaryKey;
            }
        }

        public TSecondayKey SecondaryKey
        {
            get
            {
                return secondaryKey;
            }
        }

        public TValue Value
        {
            get
            {
                return value;
            }
        }

        public KeyKeyValuePair(TPrimaryKey primaryKey, TSecondayKey secondaryKey, TValue value)
        {
            this.primaryKey = primaryKey;
            this.secondaryKey = secondaryKey;
            this.value = value;
        }

        public override string ToString()
        {
            return primaryKey.ToString() + "," + secondaryKey.ToString() + ": " + value.ToString();
        }
    }
}