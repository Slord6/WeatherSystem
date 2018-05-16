using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// Drives the wind near the player
    /// </summary>
    public class IntensityDrivenWind : IntensityDrivenBehaviour
    {
        [SerializeField]
        private WindZone windZone;
        [SerializeField]
        private float fixedMultiplier = 2.0f;
        [SerializeField]
        private WeatherManager weatherManager;
        [SerializeField]
        private float maxDegreesPerSeconds = 1f;

        private Vector3 lastKnownWind;

        private void Awake()
        {
            if(weatherManager == null)
            {
                weatherManager = FindObjectOfType<WeatherManager>();
            }
        }

        protected override void ActivationBehaviour()
        {
            windZone.gameObject.SetActive(true);
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            //We could do this ourselves, but the weather manager already keeps track of the cumulative wind at the position we need, so we'll use that one
            Vector2 trackedWind = weatherManager.GetCumulativeWind();
            lastKnownWind = new Vector3(trackedWind.x, 0, trackedWind.y);

            Quaternion startRotation = windZone.transform.rotation;
            windZone.transform.LookAt(windZone.transform.position + lastKnownWind);
            float step = maxDegreesPerSeconds * Time.deltaTime;
            windZone.transform.rotation = Quaternion.RotateTowards(startRotation, windZone.transform.rotation, step);
                
            windZone.windMain = intensityData.intensity * 2f * fixedMultiplier;
            windZone.windTurbulence = intensityData.intensity * fixedMultiplier;

            windZone.windPulseMagnitude = windZone.windMain * 0.5f;
            windZone.windPulseFrequency = windZone.windMain * 0.01f;
        }

        protected override void FadeDelegate(float t)
        {
            UpdateWithIntensity(new IntensityData(1f - t, TemperatureVariables.TemperatureMid, HumidityVariables.HumidityMid, lastKnownWind, WeatherTypes.None));
            if(t == 1.0f)
            {
                windZone.gameObject.SetActive(false);
            }
        }
    }
}
