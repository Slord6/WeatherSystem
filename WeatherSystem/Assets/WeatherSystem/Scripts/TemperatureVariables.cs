using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    /// <summary>
    /// The available states of temperature
    /// </summary>
	public enum TemperatureVariables
	{
        TemperatureHigh,
        TemperatureMid,
        TemperatureLow
    }

    public static class TemperatureExtensions
    {
        /// <summary>
        /// Convert a raw temperature value into its enum equivalent
        /// </summary>
        /// <param name="x">The value to convert</param>
        /// <returns>The TemperatureVariables representation of the value</returns>
        public static TemperatureVariables ToTemperatureValue(this float x)
        {
            x = Mathf.Clamp01(x);
            if (x < (1.0f / 3.0f))
            {
                return TemperatureVariables.TemperatureLow;
            }
            else if (x < (2.0f / 3.0f))
            {
                return TemperatureVariables.TemperatureMid;
            }
            else
            {
                return TemperatureVariables.TemperatureHigh;
            }
        }
    }

}