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

        public override void OnActivate()
        {
            base.OnActivate();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

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
            //foreach (TemperatureHumidityPair storedPair in tempHumidityPairs)
            //{
            //    if (temperatureHumidityPair.Equals(storedPair))//if we find a match
            //    {
            //        if (activateOnMatch) //and that's what we wanted to find
            //        {
            //            return true; //...say so!
            //        }
            //        else
            //        {
            //            //otherwise keep checking to confirm no matched
            //            continue;
            //        }
            //    }
            //}
            //
            ////If we get here...
            ////When activate on match = true, we return false (didn't find matching pair + that's what we wanted)
            ////When activate on match = false, we return true (again, didn't find matching pair, but this time we did want that)
            //return !activateOnMatch;
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            if (ShouldUpdate(new TemperatureHumidityPair(intensityData.temperature, intensityData.humidity)))
            {
                base.UpdateWithIntensity(intensityData);
            }
            else
            {
                OnDeactivate();
            }
        }
    }
}
