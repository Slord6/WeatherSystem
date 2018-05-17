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
        [SerializeField]
        private float emissionRateMultiplier = 100f;
        [SerializeField]
        private float gravityMultiplier = 10f;

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
            
            emmisionModule.rateOverTime = value * emissionRateMultiplier;
            mainModule.gravityModifier = intensityData.intensity * gravityMultiplier;
        }
    }
}