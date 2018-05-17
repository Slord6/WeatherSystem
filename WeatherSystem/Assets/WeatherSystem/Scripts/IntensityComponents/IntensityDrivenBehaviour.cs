using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System;
using System.Reflection;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// A monobehaviour which uses intensity data to drive its behaviour
    /// </summary>
    public abstract class IntensityDrivenBehaviour : MonoBehaviour, IIntensityDriven, IActivatable
    {
        [SerializeField]
        private WeatherProperty propertyParent;
        [SerializeField]
        private float fadeOutTime = 10.0f;

        private IntensityData intensityData;

        protected Coroutine fadeOutCoroutine;
        private bool active = false;

        /// <summary>
        /// The most recent intensity data assigned to this object.
        /// If set, will result in the behaviour being driven by the IntensityData to be updated
        /// </summary>
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

        /// <summary>
        /// The weather property whose intesnity is used by this object.
        /// This is used for the WeatherProperty to find the object when necessary.
        /// </summary>
        public WeatherProperty PropertyParent
        {
            get
            {
                return propertyParent;
            }
        }

        /// <summary>
        /// Shortcut for IntensityData.intensity
        /// </summary>
        public float Intensity
        {
            get
            {
                return intensityData.intensity;
            }
        }

        //updated by changes to IntensityData
        protected virtual void UpdateWithIntensity(IntensityData intensityData)
        {
            Debug.LogWarning("This behaviour has no Intensity update code - " + this.name);
        }

        /// <summary>
        /// Activates the object, if not already active. If a FadeOut coroutine is running, it is stopped. ActivationBehaviour is called.
        /// </summary>
        public void OnActivate()
        {
            if (active)
            {
                return;
            }
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }
            ActivationBehaviour();
            
            active = true;
        }

        /// <summary>
        /// Deactivates the object, if not already inactive. Starts the FadeOut corouinte with FadeDelegate as the callback argument
        /// </summary>
        public void OnDeactivate()
        {
            if (!active)
            {
                return;
            }

            active = false;
            fadeOutCoroutine = StartCoroutine(FadeOut(fadeOutTime, FadeDelegate));
        }

        /// <summary>
        /// Gradually reduces the object to the off state
        /// </summary>
        /// <param name="time">The time that should be taken to reach fully 'off'</param>
        /// <param name="fadeCallback">The callback which to pass the extent which to turn off, over multiple frames, which will do the actual deactivation</param>
        /// <returns>IEnumerator for coroutine</returns>
        protected IEnumerator FadeOut(float time, Action<float> fadeCallback)
        {
            yield return null;
            float cumulativeTime = 0.0f;

            while (cumulativeTime < time)
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
        /// <param name="t">An interpolation value between 0 and 1, where 0 is fully active and 1 is inactive</param>
        protected virtual void FadeDelegate(float t)
        {
        }
    }
}