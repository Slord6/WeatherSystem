using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    /// <summary>
    /// The available states of humidity
    /// </summary>
	public enum HumidityVariables
    {
        HumidityHigh,
        HumidityMid,
        HumidityLow
    }

    /// <summary>
    /// Extension methods for HumidtyVariables
    /// </summary>
    public static class HumidityExtensions
    {
        /// <summary>
        /// Convert a raw humidity value into its enum equivalent
        /// </summary>
        /// <param name="x">The value to convert</param>
        /// <returns>The HumidityVariables representation of the value</returns>
        public static HumidityVariables ToHumidityValue(this float x)
        {
            x = Mathf.Clamp01(x);
            if (x < (1.0f / 3.0f))
            {
                return HumidityVariables.HumidityLow;
            }
            else if (x < (2.0f / 3.0f))
            {
                return HumidityVariables.HumidityMid;
            }
            else
            {
                return HumidityVariables.HumidityHigh;
            }
            
        }
    }
}