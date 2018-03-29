using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System;

namespace WeatherSystem.IntensityComponents
{
    public abstract class IntensityDrivenBehaviour : MonoBehaviour, IIntensityDriven, IActivatable
    {
        [SerializeField]
        private WeatherProperty propertyParent;

        private IntensityData intensityData;

        public IntensityData IntensityData
        {
            get
            {
                return intensityData;
            }
            set
            {
                intensityData = value;
                if (value.intensity < 0)
                {
                    intensityData.intensity = 0;
                }
                else if (value.intensity > 1)
                {
                    intensityData.intensity = 1;
                }
                UpdateWithIntensity(intensityData);
            }
        }

        public WeatherProperty PropertyParent
        {
            get
            {
                return propertyParent;
            }
        }

        public float Intensity
        {
            get
            {
                return intensityData.intensity;
            }
        }

        //updated by changes to Intensity
        protected virtual void UpdateWithIntensity(IntensityData intensityData)
        {
            Debug.LogWarning("This behaviour has no Intensity update code - " + this.name);
        }

        public virtual void OnActivate()
        {
            Debug.LogWarning("This behaviour has no OnActivate update code - " + this.name);
        }

        public virtual void OnDeactivate()
        {
            Debug.LogWarning("This behaviour has no OnDeactivate update code - " + this.name);
        }
    }
}