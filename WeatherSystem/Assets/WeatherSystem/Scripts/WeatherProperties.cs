using System;
using UnityEngine;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    /// <summary>
    /// A managed collection of WeatherProperty objects
    /// </summary>
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Weather Properties")]
    public class WeatherProperties : ScriptableObject, IActivatable
	{
        [SerializeField]
        private WeatherProperty[] weatherProperties; //name must stay as seen for WeatherEventEditor

        [SerializeField]
        private ReliantWeatherProperty[] reliantWeatherProperties; //name must stay as seen for WeatherEventEditor
        
        /// <summary>
        /// Iterates over all held WeatherPropertys and updates them with the new intensity value
        /// Then notifies all reliant weather properties to update
        /// </summary>
        /// <param name="intensityData">The new intensity values</param>
        /// <param name="intensityCurves">A set of intensity curves to calculate a new iontensity value for each
        /// WeatherProperty. Must match the total number of Non-Reliant and Reliant WeatherProperties</param>
        public virtual void ApplyIntensity(IntensityData intensityData, AnimationCurve[] intensityCurves = null)
        {
            if (intensityCurves == null)
            {
                //apply changes to non reliant
                foreach (WeatherProperty weatherProperty in weatherProperties)
                {
                    weatherProperty.ApplyIntensity(intensityData);
                }

                //then notify reliant to update
                foreach (ReliantWeatherProperty reliantWeatherProperty in reliantWeatherProperties)
                {
                    //The passed intensity value doesn't matter here as RealiantWeatherPropertys calculate their own values
                    //from the parents they rely on... hence the name, but we'll pass through the other data in case that's needed
                    reliantWeatherProperty.ApplyIntensity(intensityData);
                }
            }
            else
            {
                if(intensityCurves.Length != weatherProperties.Length + reliantWeatherProperties.Length)
                {
                    Debug.LogError("Curve length mis-match (" + intensityCurves.Length + " vs " + (weatherProperties.Length + reliantWeatherProperties.Length).ToString() + "), open in the editor to refresh the curves");
                }

                for (int i = 0; i < weatherProperties.Length; i++)
                {
                    IntensityData evaluatedIntensity = new IntensityData(intensityCurves[i].Evaluate(intensityData.intensity), intensityData.temperature, intensityData.humidity, intensityData.wind, intensityData.weatherType);
                    weatherProperties[i].ApplyIntensity(evaluatedIntensity);
                }

                for (int i = 0; i < reliantWeatherProperties.Length; i++)
                {
                    int index = i + weatherProperties.Length;
                    IntensityData evaluatedIntensity = new IntensityData(intensityCurves[index].Evaluate(intensityData.intensity), intensityData.temperature, intensityData.humidity, intensityData.wind, intensityData.weatherType);
                    reliantWeatherProperties[i].ApplyIntensity(evaluatedIntensity);
                }
            }
        }

        /// <summary>
        /// Calculates the intensity that would be passed to a weather property (NOT a reliant weather property) and returns the result
        /// </summary>
        /// <param name="evaluationProperty">The property to evaluate the intensity against</param>
        /// <param name="intensityCurves">The curves used for evaluation</param>
        /// <param name="intensity">The intensity data to evaluate</param>
        /// <returns>The evaluated intensity if the weather property is in this set, null otherwise</returns>
        public virtual float? EvaluateIntensityData(WeatherProperty evaluationProperty, AnimationCurve[] intensityCurves, float intensity)
        {
            if(evaluationProperty.GetType() == typeof(ReliantWeatherProperty))
            {
                throw new ArgumentException("Cannot evaluate ReliantWeatherProperty intensity data");
            }
            for (int i = 0; i < weatherProperties.Length; i++)
            {
                if(weatherProperties[i] == evaluationProperty)
                {
                    return intensityCurves[i].Evaluate(intensity);
                }
            }
            return null;
        }

        /// <summary>
        /// Activates all held WeatherPropertys and ReliantWeatherProperties
        /// </summary>
        public void OnActivate()
        {
            foreach (WeatherProperty property in weatherProperties)
            {
                property.OnActivate();
            }
            foreach(ReliantWeatherProperty property in reliantWeatherProperties)
            {
                property.OnActivate();
            }
        }

        /// <summary>
        /// Deactivates all held WeatherPropertys
        /// </summary>
        public void OnDeactivate()
        {
            foreach (WeatherProperty property in weatherProperties)
            {
                property.OnDeactivate();
            }

            //TODO: Check if reliants should also be disabled?
        }
    }
}