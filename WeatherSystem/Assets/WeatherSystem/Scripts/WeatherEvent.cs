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
        private WeatherProperty customProperties;

        [Header("Property intensity settings")]
        [SerializeField]
        private AnimationCurve windIntensityCurve;
        [SerializeField]
        private AnimationCurve precipitationIntensityCurve;
        [SerializeField]
        private AnimationCurve debrisIntensityCurve;
        [SerializeField]
        private AnimationCurve lightingIntensityCurve;
        [SerializeField]
        private Color lightColor;
        [SerializeField]
        private AnimationCurve cloudIntensityCurve;
        [SerializeField]
        private AnimationCurve backgroundSoundCurve;
        [SerializeField]
        private AnimationCurve instanceSoundCurve;

        public WeatherTypes WeatherType
        {
            get
            {
                return weatherType;
            }
        }

        public WeatherPropertyData GetPropertyDataAtIntensity(float intensity)
        {
            WeatherPropertyData data = new WeatherPropertyData();
            data.rawIntensity = intensity;
            data.customProperties = customProperties;

            data.windIntensity = windIntensityCurve.Evaluate(intensity);
            data.precipitationIntensity = precipitationIntensityCurve.Evaluate(intensity);
            data.debrisIntensity = debrisIntensityCurve.Evaluate(intensity);
            data.lightIntensity = lightingIntensityCurve.Evaluate(intensity);
            data.cloudIntensity = cloudIntensityCurve.Evaluate(intensity);
            data.lightColor = lightColor;
            data.backgroundSoundIntensity = backgroundSoundCurve.Evaluate(intensity);
            data.instanceSoundIntensity = instanceSoundCurve.Evaluate(intensity);

            return data;
        }
    }
}