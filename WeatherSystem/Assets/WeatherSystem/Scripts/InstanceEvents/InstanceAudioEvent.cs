using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;

namespace WeatherSystem.InstanceEvents
{
    public class InstanceAudioEvent : InstanceEvent
    {
        [SerializeField]
        private AudioSource controlledAudioSource;

        public override void FadeDelegate(float t)
        {
            if (controlledAudioSource.volume != 0.0f)
            {
                controlledAudioSource.volume = 1.0f - t;
            }
        }

        public override void Activate(IntensityData intensityData)
        {
            //as intensity increases, chance of occurence also increases
            if (!controlledAudioSource.isPlaying) //only play if not already playing sound
            {
                controlledAudioSource.volume = intensityData.intensity;
                controlledAudioSource.Play();
            }

        }
    }
}