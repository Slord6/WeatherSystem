using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenAudio : IntensityDrivenBehaviour
    {
        [SerializeField]
        private AudioSource controlledAudioSource;
        
        protected override void ActivationBehaviour()
        {
            base.ActivationBehaviour();
            controlledAudioSource.enabled = true;
            controlledAudioSource.Play();
        }

        protected override void FadeDelegate(float t)
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