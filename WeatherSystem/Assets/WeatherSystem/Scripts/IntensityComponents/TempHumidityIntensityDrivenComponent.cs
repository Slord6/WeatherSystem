using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeatherSystem.IntensityComponents
{
    public class TempHumidityIntensityDrivenComponent : IntensityDrivenBehaviour
    {
        [SerializeField]
        private TemperatureHumidityPair[] tempHumidityPairs;
        [SerializeField]
        [Tooltip("If true, the above list is used as a list of pairs we want to see to be able to activate. If false, the list is used as pairs that, if seen, will cause no activation.")]
        private bool activateOnMatch;

        /// <summary>
        /// Checks to see if the tempHumidtyPairs array contains the given pair and whether activation should occur on match or no match
        /// </summary>
        /// <param name="temperatureHumidityPair">The pair to check</param>
        /// <returns>
        /// If the list contains the value and the object should update on match returns true. 
        /// If the list contains the value and the object should not update on match returns false. 
        /// If the list doesn't contain the value and the object should update on match returns false. 
        /// If the list doesn't contain the value and the object should not update on match return true</returns>
        protected bool ShouldUpdate(TemperatureHumidityPair temperatureHumidityPair)
        {
            bool shouldUpdate = tempHumidityPairs.Contains(temperatureHumidityPair) == activateOnMatch;
            return shouldUpdate;
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            if (ShouldUpdate(new TemperatureHumidityPair(intensityData.temperature, intensityData.humidity)))
            {
                ConditionalUpdateWithIntensity(intensityData);
            }
            else
            {
                OnDeactivate();
            }
        }

        /// <summary>
        /// Logic for applying intensity data. This method is called when updating intensity and ShouldUpdate is true
        /// </summary>
        /// <param name="intensityData">The intensity data</param>
        protected virtual void ConditionalUpdateWithIntensity(IntensityData intensityData)
        {
            base.UpdateWithIntensity(intensityData);
        }
    }
}
