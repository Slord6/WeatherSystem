using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    /// <summary>
    /// Scriptable object holding objects related to a specific weather pattern
    /// </summary>
    [CreateAssetMenu(menuName = "Weather System/Weather Event")]
    public class WeatherEvent : IntensityScriptableObject
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

        public WeatherTypes WeatherType
        {
            get
            {
                return weatherType;
            }
        }
    }
}