using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem
{
    [System.Serializable]
    public class TemperatureHumidityPair
    {
        [SerializeField]
        private TemperatureVariables temperature;
        [SerializeField]
        private HumidityVariables humidity;

        public TemperatureVariables Temperature
        {
            get
            {
                return temperature;
            }
        }

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
