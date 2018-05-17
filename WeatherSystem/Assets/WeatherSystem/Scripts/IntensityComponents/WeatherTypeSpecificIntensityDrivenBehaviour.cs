using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// Intensity driven behaviour for which the condition for updating is a specific weather type being active
    /// </summary>
    public class WeatherTypeSpecificIntensityDrivenBehaviour : ConditionalIntensityDrivenComponent
    {
        [SerializeField]
        private WeatherTypes[] validWeatherTypes;

        /// <summary>
        /// Should the behaviour update?
        /// </summary>
        /// <param name="intensityData">The intensity data that will be updated with</param>
        /// <returns>True if validWeatherTypes contains the WeatherType set in the intensityData object</returns>
        protected override bool ShouldUpdate(IntensityData intensityData)
        {
            try
            {
                return validWeatherTypes.Contains(intensityData.weatherType);
            }catch(System.Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
    }
}
