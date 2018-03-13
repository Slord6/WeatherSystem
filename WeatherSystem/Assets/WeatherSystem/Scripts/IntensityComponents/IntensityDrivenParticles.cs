using UnityEngine;
using System.Collections;
using System;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenParticles : IntensityDrivenBehaviour
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

        protected override void UpdateWithIntensity(float intensity)
        {
            float value = precipitationRateCurve.Evaluate(intensity);
            
            emmisionModule.rateOverTime = value;
            mainModule.gravityModifier = intensity * 10.0f;
        }
    }
}