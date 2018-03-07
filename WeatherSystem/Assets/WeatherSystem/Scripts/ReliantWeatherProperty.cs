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
        public override void ApplyIntensity(float intensity)
        {
            float totalIntensity = 0.0f;
            for (int i = 0; i < intensityParentWeightings.Length; i++)
            {
                totalIntensity += intensityParentWeightings[i].weightingCurve.Evaluate(intensity);
            }

            totalIntensity /= intensityParentWeightings.Length;

            //Then apply calculated data
            base.ApplyIntensity(intensity);
        }

        protected Color AverageColor(Color firstColor, Color secondColor)
        {
            return new Color((firstColor.r + secondColor.r) / 2.0f, (firstColor.g + secondColor.g) / 2.0f, (firstColor.b + secondColor.b) / 2.0f, (firstColor.a + secondColor.a) / 2.0f);
        }
    }
}