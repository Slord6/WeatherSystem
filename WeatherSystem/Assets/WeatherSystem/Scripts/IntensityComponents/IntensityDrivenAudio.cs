using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenAudio : IntensityDrivenBehaviour
    {
        [SerializeField]
        private AudioSource controlledAudioSource;

        public override void OnActivate()
        {
            controlledAudioSource.enabled = true;
            controlledAudioSource.Play();
        }

        public override void OnDeactivate()
        {
            controlledAudioSource.Stop();
            controlledAudioSource.enabled = false;
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            controlledAudioSource.volume = intensityData.intensity;
        }
    }
}