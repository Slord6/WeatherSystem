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

        [SerializeField]
        private WeatherProperty[] properties;

        public WeatherTypes WeatherType
        {
            get
            {
                return weatherType;
            }
        }

        public new float Intensity
        {
            get
            {
                return base.Intensity;
            }
            set
            {
                base.Intensity = value;

                if (properties != null && properties.Length > 0)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        properties[i].Intensity = value;
                    }
                }
            }
        }
    }
}