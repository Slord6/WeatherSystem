using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// An intensity driven behaviour that only applies the intensity change if some condition is met. If the condition is not met the behaviour is deactivated
    /// </summary>
    public class ConditionalIntensityDrivenComponent : IntensityDrivenBehaviour
    {
        protected override void ActivationBehaviour()
        {
            base.ActivationBehaviour();
        }

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
                OnActivate(); //will only apply if not already active
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
            Debug.Log("Conditional update ran for " + this.name);
            base.UpdateWithIntensity(intensityData);
        }
    }
}