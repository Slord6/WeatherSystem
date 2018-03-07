using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenFog : IntensityDrivenBehaviour
	{
        private float initialFog;

        public override void OnActivate()
        {
            initialFog = RenderSettings.fogDensity;
            base.OnActivate();
        }

        protected override void UpdateWithIntensity(float intensity)
        {
            RenderSettings.fogDensity = intensity;
        }

        public override void OnDeactivate()
        {
            RenderSettings.fogDensity = initialFog;
            base.OnDeactivate();
        }
    }
}