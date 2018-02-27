using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    /// <summary>
    /// The central manager for weather
    /// Operates in two modes; procedural and manual.
    /// Procedural mode uses perlin noise values to transit9on between WeatherSets and WeatherEvents with gradual changes over time driven by procedural 'wind'. [Partial implementation]
    /// Manual mode gradually transitions between WeatherSets, and WeatherEvents as described by a provided WeatherCycle [Not implemented]
    /// </summary>
	public class WeatherManager : MonoBehaviour
	{
        [HideInInspector] //This is so the custom editor can draw correctly
        public WeatherMode procedural = WeatherMode.Procedural;

        [SerializeField]
        private Transform weatherQueryLocation;
        
        #region Manual mode fields
        //Fields with the attribute "Manual" are only drawn when
        //WeatherMode is 'Manual'
        [SerializeField]
        [Manual]
        private List<WeatherSet> weatherEvents;
        #endregion

        #region Procedural mode fields
        //Fields with the attribute "Procedural" are only drawn when
        //WeatherMoide is 'Procedural'
        [SerializeField]
        [Procedural]
        private List<WeatherSet> weatherSets;

        [SerializeField]
        [Procedural]
        private ProceduralWeatherLookup proceduralWeatherLookup;

        [SerializeField]
        [Procedural]
        private AnimationCurve transitionCurve;

        [SerializeField]
        [Procedural]
        private Vector2 worldSize = new Vector2(1000.0f,1000.0f);

        [SerializeField]
        [Procedural]
        private float windStrength = 1.0f;

        [SerializeField]
        [Procedural]
        private float proceduralScale = 25.0f;
        #endregion

        #region Wind values
        private float trackedX = 0.0f;
        private float trackedY = 0.0f;
        #endregion
        
        private Coroutine transitionCoroutine;
        private WeatherTypes weatherLastFrame = WeatherTypes.None;
        private WeatherSet activeWeatherSet;

        // Use this for initialization
        protected virtual void Start ()
		{
            weatherLastFrame = GetWeather();
            //Temporary
            activeWeatherSet = weatherSets[0];
        }

        /// <summary>
        /// Get the weather using the default tracked transform for position and time since level load as time
        /// </summary>
        /// <returns>The weather at the position</returns>
        public WeatherTypes GetWeather()
        {
            Vector2 position = new Vector2(weatherQueryLocation.position.x, weatherQueryLocation.position.z); // x and z becase player moves laterally in the x/z plane
            return GetWeather(position, Time.timeSinceLevelLoad);
        }

        /// <summary>
        /// Get the weather for a particular time and place
        /// </summary>
        /// <param name="weatherQueryLocation">The position at which to get the weather</param>
        /// <param name="time">The time at which the weather should be queryied</param>
        /// <returns>The WeatherTypes for that position at that time</returns>
        public WeatherTypes GetWeather(Vector2 weatherQueryLocation, float time)
        {
            float timeScale = 0.1f; //To stop weather changes happening too quickly, we scale the time

            Vector2 wind = Generators.GetDirectionalNoise(weatherQueryLocation.x, weatherQueryLocation.y, worldSize.x, worldSize.y, proceduralScale, time * timeScale);
            wind *= windStrength;
            trackedX += wind.x;
            trackedY += wind.y;

            TemperatureVariables temperature = Generators.GetTemperatureValue(weatherQueryLocation.x + trackedX, weatherQueryLocation.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f).ToTemperatureValue();
            HumidityVariables humidity =Generators.GetHumidityValue(weatherQueryLocation.x + trackedX, weatherQueryLocation.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f).ToHumidityValue();

            WeatherTypes currentWeather;
            if (proceduralWeatherLookup.LookupTable.TryGetValue(humidity, temperature, out currentWeather))
            {
                return currentWeather;
            }
            else
            {
                Debug.LogError("Procedural lookup table does not contain info for " + humidity + " and " + temperature);
                return WeatherTypes.None;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (transitionCoroutine == null)
            {   
                WeatherTypes currentWeather = GetWeather();
                if (currentWeather != weatherLastFrame) //If the weather changed this frame
                {
                    WeatherEvent currentWeatherEvent = WeatherEventFromWeatherType(weatherLastFrame);
                    WeatherEvent newWeatherEvent = WeatherEventFromWeatherType(currentWeather);
                    if (currentWeatherEvent == null)
                    {
                        Debug.LogError("No weather event set for " + weatherLastFrame);
                        return;
                    }
                    if(newWeatherEvent == null)
                    {
                        Debug.LogError("No weather event set for " + currentWeather);
                        return;
                    }
                    transitionCoroutine = StartCoroutine(Transition(currentWeatherEvent, newWeatherEvent));
                    weatherLastFrame = currentWeather;
                }
            }

        }

        private WeatherEvent WeatherEventFromWeatherType(WeatherTypes weather)
        {
            for (int i = 0; i < activeWeatherSet.WeatherEvents.Length; i++)
            {
                if(activeWeatherSet.WeatherEvents[i].WeatherType == weather)
                {
                    return activeWeatherSet.WeatherEvents[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Move between two weather events by transferring intensity from one to the other as defined by our transitionCurve
        /// </summary>
        /// <param name="currentWeatherEvent">The current weather event to transition from</param>
        /// <param name="nextWeatherEvent">The weather event to transition to</param>
        /// <param name="time">The time the transition should take</param>
        private IEnumerator Transition(WeatherEvent currentWeatherEvent, WeatherEvent nextWeatherEvent)
        {
            float time = transitionCurve.keys[transitionCurve.length - 1].time; //time of the last keyframe is the length of the entire curve

            float evaluationValue = 0.0f;
            while(evaluationValue <= time)
            {
                //Calculate a value between zero and 1 based on the length of the curve and the
                //(value between 0 and the max time of the curve)
                float normalisedEvaulationValue = evaluationValue / time;
                float curveOutput = transitionCurve.Evaluate(normalisedEvaulationValue);

                //Increase intensity of new weather whilst
                //proportionally decreasing the instensity of the other
                nextWeatherEvent.Intensity = curveOutput;
                currentWeatherEvent.Intensity = 1 - curveOutput;

                evaluationValue += Time.deltaTime;
                yield return null;
            }
            Debug.Log("New weather: " + nextWeatherEvent.WeatherType.ToString());
            transitionCoroutine = null;
        }
	}
}