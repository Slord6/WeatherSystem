using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem
{
    /// <summary>
    /// Intensity and weather related data container that is passed down the UpdateIntensity heirarchy
    /// </summary>
    public class IntensityData
    {
        /// <summary>
        /// The intensity of the weather event
        /// </summary>
        public float intensity;
        /// <summary>
        /// The temperature
        /// </summary>
        public TemperatureVariables temperature;
        /// <summary>
        /// The humidity
        /// </summary>
        public HumidityVariables humidity;
        /// <summary>
        /// The normalised wind
        /// </summary>
        public Vector2 wind;
        /// <summary>
        /// The currently active weather type
        /// </summary>
        public WeatherTypes weatherType;

        public IntensityData(float intensity, TemperatureVariables temperature, HumidityVariables humidity, Vector2 wind, WeatherTypes weatherType)
        {
            this.intensity = intensity;
            this.temperature = temperature;
            this.humidity = humidity;
            this.wind = wind;
            this.weatherType = weatherType;
        }
    }
}
