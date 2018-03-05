using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Weather Property")]
    public class WeatherProperty : IntensityScriptableObject
	{
        [SerializeField]
        [Tooltip("Given an intensity value, what (normalised) value should be applied to the values on this property")]
        private AnimationCurve intensityCurve;

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
        private Light worldLight;
        [SerializeField]
        private Color lightColor;
        
    }
}