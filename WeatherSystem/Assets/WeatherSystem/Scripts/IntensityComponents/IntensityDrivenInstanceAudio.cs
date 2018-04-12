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

        protected override void ActivationBehaviour()
        {
            audioSource.enabled = true;
        }

        protected override void FadeDelegate(float t)
        {
            if (audioSource.volume != 0.0f)
            {
                audioSource.volume = 1.0f - t;
            }
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            float randomNumber = Random.Range(0.0f, instanceChance);

            if (randomNumber < instanceChance)
            {
                audioSource.volume = intensityData.intensity;
                audioSource.Play();
            }
        }
    }
}