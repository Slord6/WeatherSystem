using System;
using UnityEngine;
using WeatherSystem.Internal;

namespace WeatherSystem
{
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
        /// <param name="intensity">The new intensity values</param>
        /// <param name="intensityCurves">A set of intensity curves to calculate a new iontensity value for each
        /// WeatherProperty. Must match the total number of Non-Reliant and Reliant WeatherProperties</param>
        public virtual void ApplyIntensity(float intensity, AnimationCurve[] intensityCurves = null)
        {
            if (intensityCurves == null)
            {
                //apply changes to non reliant
                foreach (WeatherProperty weatherProperty in weatherProperties)
                {
                    weatherProperty.ApplyIntensity(intensity);
                }

                //then notify reliant to update
                foreach (ReliantWeatherProperty reliantWeatherProperty in reliantWeatherProperties)
                {
                    //The passed value doesn't matter here as RealiantWeatherPropertys calculate their own values
                    //from the parents they rely on... hence the name
                    reliantWeatherProperty.ApplyIntensity(0.0f);
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
                    weatherProperties[i].ApplyIntensity(intensityCurves[i].Evaluate(intensity));
                }

                for (int i = 0; i < reliantWeatherProperties.Length; i++)
                {
                    int index = i + weatherProperties.Length;
                    reliantWeatherProperties[i].ApplyIntensity(intensityCurves[index].Evaluate(intensity));
                }
            }
        }

        //late apply intensity

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

        public void OnDeactivate()
        {
            foreach (WeatherProperty property in weatherProperties)
            {
                property.OnDeactivate();
            }
        }
    }
}