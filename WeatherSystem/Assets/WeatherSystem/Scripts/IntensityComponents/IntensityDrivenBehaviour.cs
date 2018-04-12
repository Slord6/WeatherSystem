using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System;
using System.Reflection;

namespace WeatherSystem.IntensityComponents
{
    public abstract class IntensityDrivenBehaviour : MonoBehaviour, IIntensityDriven, IActivatable
    {
        [SerializeField]
        private WeatherProperty propertyParent;
        [SerializeField]
        private float fadeOutTime = 10.0f;

        private IntensityData intensityData;

        protected Coroutine fadeOutCoroutine;
        private bool active = false;

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

        public void OnActivate()
        {
            if (active)
            {
                return;
            }

            if(fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }
            active = true;
            ActivationBehaviour();
        }

        public void OnDeactivate()
        {
            if (!active)
            {
                return;
            }
            active = false;
            fadeOutCoroutine = StartCoroutine(FadeOut(fadeOutTime, FadeDelegate));
        }

        protected IEnumerator FadeOut(float time, Action<float> fadeCallback)
        {
            float cumulativeTime = 0.0f;

            while(cumulativeTime < time)
            {
                fadeCallback.Invoke(cumulativeTime / time);
                yield return null;
                cumulativeTime += Time.deltaTime;
            }
            fadeCallback.Invoke(1.0f);
        }

        protected virtual void ActivationBehaviour()
        {
        }

        /// <summary>
        /// A delegate that is called multiple times in order to gradually deactivate the behaviour
        /// </summary>
        /// <param name="t">An interpolation value between 0 and 1 where 0 is fully active and 1 is inactive</param>
        protected virtual void FadeDelegate(float t)
        {
        }
    }
}