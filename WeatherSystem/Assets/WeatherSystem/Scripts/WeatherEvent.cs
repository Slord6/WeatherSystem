using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    /// <summary>
    /// Scriptable object holding objects related to a specific weather pattern
    /// </summary>
    [CreateAssetMenu(menuName = "Weather System/Weather Event")]
    public class WeatherEvent : ScriptableObject
	{
        [Header("Identifier")]
        [SerializeField]
        private WeatherTypes weatherType;
        
        [Header("Audio")]
        [SerializeField]
        private AudioClip backgroundSound;
        [SerializeField]
        private AudioClip[] instanceSounds;

        [Header("Visuals")]
        [SerializeField]
        private ParticleSystem particleSystem;
        [SerializeField]
        private Shader shader;

        [SerializeField]
        [Range(0,1)]
        private float intensity = 1.0f;

        public WeatherTypes WeatherType
        {
            get
            {
                return weatherType;
            }
        }

        /// <summary>
        /// The intensity of this weather event at this instance
        /// </summary>
        public float Intensity
        {
            get
            {
                return intensity;
            }
            set
            {
                intensity = value;
                if (intensity > 1)
                {
                    intensity = 1;
                }
                else if(intensity < 0)
                {
                    intensity = 0;
                }
            }
        }
    }
}