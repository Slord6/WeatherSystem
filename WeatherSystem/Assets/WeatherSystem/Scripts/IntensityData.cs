using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem
{
    public class IntensityData
    {
        public float intensity;
        public TemperatureVariables temperature;
        public HumidityVariables humidity;

        public IntensityData(float intensity, TemperatureVariables temperature, HumidityVariables humidity)
        {
            this.intensity = intensity;
            this.temperature = temperature;
            this.humidity = humidity;
        }
    }
}
