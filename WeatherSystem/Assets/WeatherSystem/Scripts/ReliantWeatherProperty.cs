using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System.Collections.Generic;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Reliant Weather Property")]
    public class ReliantWeatherProperty : WeatherProperty
	{
        [Header("Reliant intensity settings")]
        [SerializeField]
        private RelianceWeighting[] intensityParentWeightings;
        
        /// <summary>
        /// Calculates an intensity from the parents intensity, weighted by curves,
        /// then applies that intensity value to all IntensityComponentCurves
        /// </summary>
        /// <param name="intensity">An ignored intensity value, as it is calculated from the parents. Can be anything</param>
        public override void ApplyIntensity(IntensityData intensityData)
        {
            IntensityData totalIntensity = new IntensityData(0.0f, intensityData.temperature, intensityData.humidity, intensityData.wind);
            for (int i = 0; i < intensityParentWeightings.Length; i++)
            {
                totalIntensity.intensity += intensityParentWeightings[i].weightingCurve.Evaluate(intensityData.intensity);
            }

            totalIntensity.intensity /= (float)intensityParentWeightings.Length;

            //Then apply calculated data
            base.ApplyIntensity(totalIntensity);
        }

        protected Color AverageColor(Color firstColor, Color secondColor)
        {
            return new Color((firstColor.r + secondColor.r) / 2.0f, (firstColor.g + secondColor.g) / 2.0f, (firstColor.b + secondColor.b) / 2.0f, (firstColor.a + secondColor.a) / 2.0f);
        }
    }
}