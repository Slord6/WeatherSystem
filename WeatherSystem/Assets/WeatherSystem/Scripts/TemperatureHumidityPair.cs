using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem
{
    /// <summary>
    /// Data container for a collection containing one TemperatureVariables and one HumidityVariables enum
    /// </summary>
    [System.Serializable]
    public class TemperatureHumidityPair
    {
        [SerializeField]
        private TemperatureVariables temperature;
        [SerializeField]
        private HumidityVariables humidity;

        /// <summary>
        /// The stored temperature value
        /// </summary>
        public TemperatureVariables Temperature
        {
            get
            {
                return temperature;
            }
        }

        /// <summary>
        /// The stored humidity value
        /// </summary>
        public HumidityVariables Humidity
        {
            get
            {
                return humidity;
            }
        }

        public TemperatureHumidityPair(TemperatureVariables temperature, HumidityVariables humidity)
        {
            this.temperature = temperature;
            this.humidity = humidity;
        }

        public override int GetHashCode()
        {
            return temperature.GetHashCode() + humidity.GetHashCode();
        }

        /// <summary>
        /// Confirm the given obect is equivalent to this object
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if equivalent, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(TemperatureHumidityPair))
            {
                TemperatureHumidityPair other = (TemperatureHumidityPair)obj;
                if(other.Temperature == this.Temperature && other.Humidity == this.Humidity)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

}
