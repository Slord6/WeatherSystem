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

        /// <summary>
        /// The type of wether this WeatherEvent deals with
        /// </summary>
        public WeatherTypes WeatherType
        {
            get
            {
                return weatherType;
            }
        }

        /// <summary>
        /// The weather properties managed by this weather event
        /// </summary>
        public WeatherProperties Properties
        {
            get
            {
                return customProperties;
            }
        }

        /// <summary>
        /// The intensity modification curves for weather properties
        /// </summary>
        public AnimationCurve[] WeatherPropertiesIntensityCurves
        {
            get
            {
                return curves;
            }
        }

        /// <summary>
        /// The current Intensity data for this WeatherEvent
        /// Setting this value will ApplyIntensity() on the WeatherProperties object managed by this WeatherEvent
        /// </summary>
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

        /// <summary>
        /// Activation behaviour
        /// Activates the WeatherProperties object
        /// </summary>
        public void OnActivate()
        {
            customProperties.OnActivate();
        }

        /// <summary>
        /// Deactivation behaviour
        /// Deactivates the WeatherProperties object
        /// </summary>
        public void OnDeactivate()
        {
            customProperties.OnDeactivate();
        }
    }
}