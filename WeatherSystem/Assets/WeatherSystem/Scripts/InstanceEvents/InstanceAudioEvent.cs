using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;

namespace WeatherSystem.InstanceEvents
{
    /// <summary>
    /// An instance event that plays an AudioSource
    /// </summary>
    public class InstanceAudioEvent : InstanceEvent
    {
        [SerializeField]
        private AudioSource controlledAudioSource;

        /// <summary>
        /// Sets the volume to t, as long as that reduces the volume
        /// </summary>
        /// <param name="t">Fadeout value</param>
        public override void FadeDelegate(float t)
        {
            if (controlledAudioSource.volume != 0.0f && t < controlledAudioSource.volume)
            {
                controlledAudioSource.volume = 1.0f - t;
            }
        }

        /// <summary>
        /// Sets the volume to the intensity and plays the audio source
        /// </summary>
        /// <param name="intensityData">The related intensity value to this activation</param>
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