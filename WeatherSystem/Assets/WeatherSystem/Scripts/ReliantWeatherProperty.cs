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

        public override void ApplyPropertyData(WeatherPropertyData data)
        {
            //Weighted total from all parents
            WeatherPropertyData calulatedData = new WeatherPropertyData();
            calulatedData.lightColor = intensityParentWeightings[0].intensityParent.LastAssignedPropertyData.lightColor;
            for (int i = 0; i < intensityParentWeightings.Length; i++)
            {
                RelianceWeighting currentRelianceWeighting = intensityParentWeightings[i];
                WeatherPropertyData currentParentData = currentRelianceWeighting.intensityParent.LastAssignedPropertyData;

                calulatedData.rawIntensity += currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.rawIntensity);
                calulatedData.backgroundSoundIntensity += currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.backgroundSoundIntensity);
                calulatedData.cloudIntensity += currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.cloudIntensity);
                calulatedData.debrisIntensity += currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.debrisIntensity);
                calulatedData.instanceSoundIntensity += currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.instanceSoundIntensity);
                calulatedData.lightColor = AverageColor(calulatedData.lightColor, currentParentData.lightColor);
                calulatedData.lightIntensity = currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.lightIntensity);
                calulatedData.precipitationIntensity = currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.precipitationIntensity);
                calulatedData.windIntensity = currentRelianceWeighting.weightingCurve.Evaluate(currentParentData.windIntensity);
            }

            //Then average to get normalised values
            int length = intensityParentWeightings.Length;
            calulatedData.rawIntensity /= length;
            calulatedData.backgroundSoundIntensity /= length;
            calulatedData.cloudIntensity /= length;
            calulatedData.debrisIntensity /= length;
            calulatedData.instanceSoundIntensity /= length;
            calulatedData.lightIntensity /= length;
            calulatedData.precipitationIntensity /= length;
            calulatedData.windIntensity /= length;

            //What to do with custom properties?

            //Then apply calculated data
            base.ApplyPropertyData(calulatedData);
        }

        protected Color AverageColor(Color firstColor, Color secondColor)
        {
            return new Color((firstColor.r + secondColor.r) / 2.0f, (firstColor.g + secondColor.g) / 2.0f, (firstColor.b + secondColor.b) / 2.0f, (firstColor.a + secondColor.a) / 2.0f);
        }
    }
}