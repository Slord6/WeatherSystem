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
        public Vector2 wind;

        public IntensityData(float intensity, TemperatureVariables temperature, HumidityVariables humidity, Vector2 wind)
        {
            this.intensity = intensity;
            this.temperature = temperature;
            this.humidity = humidity;
            this.wind = wind;
        }
    }
}
