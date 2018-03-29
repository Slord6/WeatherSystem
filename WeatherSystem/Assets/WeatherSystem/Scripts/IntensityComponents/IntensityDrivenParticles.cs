using UnityEngine;
using System.Collections;
using System;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenParticles : TempHumidityIntensityDrivenComponent
	{
        [SerializeField]
        private new ParticleSystem particleSystem;
        [SerializeField]
        private bool manageChildParticleSystems;
        [SerializeField]
        private AnimationCurve precipitationRateCurve;

        private ParticleSystem.EmissionModule emmisionModule;
        private ParticleSystem.MainModule mainModule;
        

        private void Awake()
        {
            emmisionModule = particleSystem.emission;
            mainModule = particleSystem.main;
        }

        public override void OnActivate()
        {
            emmisionModule.enabled = true;
        }

        public override void OnDeactivate()
        {
            emmisionModule.enabled = false;
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            if (ShouldUpdate(new TemperatureHumidityPair(intensityData.temperature, intensityData.humidity)))
            {
                float value = precipitationRateCurve.Evaluate(intensityData.intensity);

                emmisionModule.rateOverTime = value * 100;
                mainModule.gravityModifier = intensityData.intensity * 10.0f;
            }
            else
            {
                OnDeactivate();
            }
        }
    }
}