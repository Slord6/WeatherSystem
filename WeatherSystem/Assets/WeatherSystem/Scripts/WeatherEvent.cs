using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System;

namespace WeatherSystem
{
    /// <summary>
    /// Scriptable object holding WeatherProperties related to a specific weather pattern
    /// </summary>
    [CreateAssetMenu(menuName = "Weather System/Weather Event")]
    public class WeatherEvent : IntensityScriptableObject, IActivatable
    {
        [Header("Identifier")]
        [SerializeField]
        private WeatherTypes weatherType;

        [SerializeField]
        private WeatherProperties customProperties; //this name cannot be changed, required for editor
        
        [SerializeField]
        [HideInInspector]
        private AnimationCurve[] curves;

        public WeatherTypes WeatherType
        {
            get
            {
                return weatherType;
            }
        }

        public override float Intensity
        {
            get
            {
                return base.Intensity;
            }
            set
            {
                base.Intensity = value;
                if (customProperties != null)
                {
                    customProperties.ApplyIntensity(Intensity, curves);
                }
                else
                {
                    Debug.LogWarning("No properties set for WeatherEvent - " + name);
                }
            }
        }

        public void OnActivate()
        {
            customProperties.OnActivate();
        }

        public void OnDeactivate()
        {
            customProperties.OnDeactivate();
        }
    }
}