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
        [Range(0.0f,1.0f)]
        private float snowCumulationMultiplier = 0.1f;
        private float trackedIntensity = 0.0f;

        private int updateCount = 0;
        private float timeActivated = 0.0f;
        
        private float initialFadeBottom;
        private float initialFadeTop;

        protected override void ActivationBehaviour()
        {
            trackedIntensity = 0.0f;
            updateCount = 1;
            timeActivated = Time.timeSinceLevelLoad;
            snowController.Enable();
        }

        protected override void FadeDelegate(float t)
        {
            if (t == 0.0f)
            {
                initialFadeBottom = snowController.BottomThreshold;
                initialFadeTop = snowController.TopThreshold;
            }

            if (snowController.BottomThreshold != 1.0f)
            {
                snowController.BottomThreshold = t + initialFadeBottom;
            }
            if (snowController.TopThreshold != 0.0f)
            {
                snowController.TopThreshold = 1.0f - (t + initialFadeTop);
            }

            if(t == 1.0f)
            {
                snowController.Disable();
            }
        }

        protected override void ConditionalUpdateWithIntensity(IntensityData intensityData)
        {
            updateCount++;
            trackedIntensity += intensityData.intensity;
            float timeSinceActivated = Time.timeSinceLevelLoad - timeActivated;

            //snow value = average intensity per update by time * multiplier
            float buildUpValue = (trackedIntensity / (float)updateCount) * timeSinceActivated * snowCumulationMultiplier;
            snowController.BottomThreshold = 1.0f - buildUpValue;

            if (snowController.BottomThreshold == 0.0f) //Build up at max, so we start increasing top threshold
            {
                snowController.TopThreshold = (trackedIntensity / (float)updateCount) * snowCumulationMultiplier;
            }
            else
            {
                if (snowController.TopThreshold != 1.0f) //if top threshold isn't at min, need to decrease it when bottom threshold isn't at max
                {
                    snowController.TopThreshold -= snowController.TopThreshold * 0.1f;
                }
            }
        }
    }

}