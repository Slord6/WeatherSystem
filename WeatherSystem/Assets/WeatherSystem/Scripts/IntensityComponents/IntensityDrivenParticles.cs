using UnityEngine;
using System.Collections;
using System;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// A particle controller driven by intensity data
    /// </summary>
	public class IntensityDrivenParticles : WeatherTypeSpecificIntensityDrivenBehaviour
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

        protected override void ActivationBehaviour()
        {
            emmisionModule.enabled = true;
        }

        protected override void FadeDelegate(float t)
        {
            base.FadeDelegate(t);
            emmisionModule.enabled = false;
        }

        /// <summary>
        /// Updates emmission rates and the gravity modifier based on the given intensity data
        /// </summary>
        /// <param name="intensityData">Intensity data</param>
        protected override void ConditionalUpdateWithIntensity(IntensityData intensityData)
        {
            float value = precipitationRateCurve.Evaluate(intensityData.intensity);

           // emmisionModule.enabled = true;

            emmisionModule.rateOverTime = value * 100;
            mainModule.gravityModifier = intensityData.intensity * 10.0f;
        }
    }
}