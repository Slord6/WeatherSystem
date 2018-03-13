using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenInstanceAudio : IntensityDrivenAudio
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float instanceChance;

        public override void OnActivate()
        {
            audioSource.enabled = true;
        }

        public override void OnDeactivate()
        {
            audioSource.enabled = false;
        }

        protected override void UpdateWithIntensity(float intensity)
        {
            float randomNumber = Random.Range(0.0f, instanceChance);

            if (randomNumber < instanceChance)
            {
                audioSource.volume = intensity;
                audioSource.Play();
            }
        }
    }
}