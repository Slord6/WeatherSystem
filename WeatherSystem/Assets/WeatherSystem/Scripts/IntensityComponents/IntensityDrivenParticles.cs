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

        public override void OnActivate()
        {
            ParticleSystem.EmissionModule emmision = particleSystem.emission;
            emmision.enabled = true;
        }

        public override void OnDeactivate()
        {
            ParticleSystem.EmissionModule emmision = particleSystem.emission;
            emmision.enabled = false;
        }

        protected override void UpdateWithIntensity(float intensity)
        {
            ParticleSystem.EmissionModule emmision = particleSystem.emission;
            emmision.rateOverTime = 100 * intensity;
        }
    }
}