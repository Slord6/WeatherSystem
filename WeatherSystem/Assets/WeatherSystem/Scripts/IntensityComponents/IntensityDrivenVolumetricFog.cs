using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System;

namespace WeatherSystem.IntensityComponents
{
    public class IntensityDrivenVolumetricFog : IntensityDrivenBehaviour
    {
        [SerializeField]
        private new VolumetricLight light;

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            light.SkyboxExtinctionCoef = intensityData.intensity;
            light.ScatteringCoef = Scale(0.02f, 0.08f, intensityData.intensity);
            //light.ExtinctionCoef = Scale(0.003f, 0.05f, intensity); //this looks good for storms, not so much for rain
            light.ExtinctionCoef = Scale(0.003f, 0.01f, intensityData.intensity);
        }

        public override void OnActivate()
        {
            light.enabled = true;
        }

        public override void OnDeactivate()
        {
            light.enabled = false;
        }

        private float Scale(float min, float max, float scale)
        {
            float diff = max - min;
            return (diff * scale) + min;
        }
    }
}