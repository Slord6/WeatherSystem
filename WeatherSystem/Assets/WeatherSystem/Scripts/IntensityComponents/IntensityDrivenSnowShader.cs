using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.IntensityComponents
{
    public class IntensityDrivenSnowShader : TempHumidityIntensityDrivenComponent
    {
        [SerializeField]
        private ScreenSpaceSnow snowController;
        [SerializeField]
        [Range(0.0f,2.0f)]
        private float snowCumulationMultiplier = 0.1f;
        private float trackedIntensity = 0.0f;

        private int updateCount = 0;

        public override void OnActivate()
        {
            trackedIntensity = 0.0f;
            updateCount = 0;
            snowController.Enable();
        }

        public override void OnDeactivate()
        {
            snowController.Disable();
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            if (ShouldUpdate(new TemperatureHumidityPair(intensityData.temperature, intensityData.humidity)))
            {
                updateCount++;
                trackedIntensity += intensityData.intensity;

                //snow value = average intensity over time * multiplier
                snowController.BottomThreshold = 1.0f - (trackedIntensity / (float)updateCount) * snowCumulationMultiplier;
            }
            else
            {
                OnDeactivate();
            }
        }
    }

}