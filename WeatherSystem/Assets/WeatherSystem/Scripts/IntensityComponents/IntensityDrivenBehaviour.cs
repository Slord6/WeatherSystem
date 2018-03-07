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
        [SerializeField]
        private AnimationCurve intensityCurve;

        private float intensity;

        public float Intensity
        {
            get
            {
                return intensity;
            }

            set
            {
                if (value < 0)
                {
                    intensity = 0;
                }
                else if (value > 1)
                {
                    intensity = 1;
                }
                else
                {
                    intensity = value;
                }
                UpdateWithIntensity(intensity);
            }
        }

        public WeatherProperty PropertyParent
        {
            get
            {
                return propertyParent;
            }
        }

        public AnimationCurve IntensityCurve
        {
            get
            {
                return intensityCurve;
            }
        }

        //updated by changes to Intensity
        protected virtual void UpdateWithIntensity(float intensity)
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