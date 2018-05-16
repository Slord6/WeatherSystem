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

        public WeatherProperties Properties
        {
            get
            {
                return customProperties;
            }
        }

        public AnimationCurve[] WeatherPropertiesIntensityCurves
        {
            get
            {
                return curves;
            }
        }

        public override IntensityData IntensityData
        {
            get
            {
                return base.IntensityData;
            }
            set
            {
                base.IntensityData = value;
                if (customProperties != null)
                {
                    customProperties.ApplyIntensity(IntensityData, curves);
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