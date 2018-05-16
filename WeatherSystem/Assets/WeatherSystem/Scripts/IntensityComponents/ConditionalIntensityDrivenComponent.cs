using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeatherSystem.IntensityComponents
{
    public class ConditionalIntensityDrivenComponent : IntensityDrivenBehaviour
    {
        /// <summary>
        /// Checks to see if this component should update
        /// </summary>
        /// <param name="intensityData">The current intensity data</param>
        /// <returns>
        protected virtual bool ShouldUpdate(IntensityData intensityData)
        {
            return true;
        }

        /// <summary>
        /// Updates the component if ShouldUpdate returns true, otherwise deactivates
        /// </summary>
        /// <param name="intensityData">The intensity data with which to update</param>
        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            if (ShouldUpdate(intensityData))
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