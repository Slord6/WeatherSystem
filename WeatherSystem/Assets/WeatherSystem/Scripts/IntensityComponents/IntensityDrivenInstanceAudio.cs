using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenInstanceAudio : IntensityDrivenAudio
    {
        [SerializeField]
        [Range(0.0f, 0.1f)] //between 0% and 10%
        private float instanceChance = 0.0005f; //default of 1/500th %

        protected override void ActivationBehaviour()
        {
            base.ActivationBehaviour();
        }

        protected override void FadeDelegate(float t)
        {
            if (controlledAudioSource.volume != 0.0f)
            {
                controlledAudioSource.volume = 1.0f - t;
            }
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            //as intensity increases, chance of occurence also increases
            float randomNumber = Random.Range(0.0f, 1.0f - intensityData.intensity);

            if (randomNumber < instanceChance)
            {
                if (!controlledAudioSource.isPlaying) //only play if not already playing sound
                {
                    controlledAudioSource.volume = intensityData.intensity;
                    controlledAudioSource.Play();
                }
            }
        }
    }
}