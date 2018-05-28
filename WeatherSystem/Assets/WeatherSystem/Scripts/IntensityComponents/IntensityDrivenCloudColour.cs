using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.IntensityComponents
{
    public class IntensityDrivenCloudColour : IntensityDrivenBehaviour
    {
        [SerializeField]
        private Light light;

        private Color idealColor;


        protected override void ActivationBehaviour()
        {
            idealColor = light.color;
        }


        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            light.intensity = 1f - (0.01f + intensityData.intensity * 0.99f);
            light.color = Color.Lerp(idealColor, Color.black, intensityData.intensity);
        }

        protected override void FadeDelegate(float t)
        {
            base.FadeDelegate(t);
            //this lerp is a bit dodge
            light.color = Color.Lerp(light.color, idealColor, t);
        }
    }
}
