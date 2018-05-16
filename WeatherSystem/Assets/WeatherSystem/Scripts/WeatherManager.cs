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
    [RequireComponent(typeof(TimeExtension))]
	public class WeatherManager : MonoBehaviour
	{
        [HideInInspector] //This is so the custom editor can draw correctly
        public WeatherMode procedural = WeatherMode.Procedural; //name needs changing to be more descriptive, editor may need updating also

        [SerializeField]
        [Tooltip("The position at which data is generated for weather updates (usually the player)")]
        private Transform weatherQueryLocation;
        
        [SerializeField]
        private List<WeatherSet> weatherSets;
        
        [SerializeField]
        private AnimationCurve transitionCurve;

        [SerializeField]
        [Tooltip("This is used in procedural mode to lookup weather events based on generated temperature and humidity values. In manual mode it is used in reverse to get temperature and humidity values from a weather event")]
        private ProceduralWeatherLookup proceduralWeatherLookup;

        #region Manual mode fields
        //Fields with the attribute "Manual" are only drawn when
        //WeatherMode is 'Manual'
        [SerializeField]
        [Manual]
        private WeatherProperties weatherProperties;

        [SerializeField]
        [Manual]
        private List<EventSequenceItem> manualEventsSequence;
        
        private bool proceduralIntensity;
        private float timeSinceSequenceChange = 0.0f;
        private int eventSequenceIndex = 0;
        #endregion

        #region Procedural mode fields
        //Fields with the attribute "Procedural" are only drawn when
        //WeatherMoide is 'Procedural'

        [SerializeField]
        [Procedural]
        [Tooltip("If not zero, used as the procedural seed, otherwise one is generated")]
        private float seed = 0;

        [SerializeField]
        [Procedural]
        private Vector2 worldSize = new Vector2(1000.0f,1000.0f);

        [SerializeField]
        [Procedural]
        private float windStrength = 0.001f;

        [SerializeField]
        [Procedural]
        private float proceduralScale = 0.1f;

        [SerializeField]
        [Procedural]
        [Range(0.1f, 300f)]
        [Tooltip("Setting this to too high a value may result in strange/constant weather transitions")]
        private float weatherEventTransitionTime = 3.0f;
        #endregion

        #region Wind values
        private float trackedX = 0.0f;
        private float trackedY = 0.0f;
        #endregion

        #region Delegates
        public delegate void OnWeatherEventDelegate(WeatherChangeEventArgs args);

        public OnWeatherEventDelegate OnWeatherChangeBeginEvent;
        public OnWeatherEventDelegate OnWeatherChangeCompleteEvent;
        public OnWeatherEventDelegate OnWeatherChangeStep;
        #endregion

        private Coroutine transitionCoroutine;
        private WeatherTypes weatherLastFrame = WeatherTypes.None;
        private TemperatureVariables temperatureLastFrame;
        private HumidityVariables humidityLastFrame;
        private WeatherSet activeWeatherSet;
        
        private TimeExtension timeExtension;

        [SerializeField]
        private AnimationCurve intensityPlot = new AnimationCurve();

        void Awake()
        {
            if(seed == 0)
            {
                Generators.Seed = Random.value * 1000000.0f + Random.value;
                seed = Generators.Seed;
            }
            else
            {
                Generators.Seed = seed;
            }
            Debug.Log("Seed = " + Generators.Seed);
        }

        // Use this for initialization
        protected virtual void Start ()
        {
            timeExtension = GetComponent<TimeExtension>();

            //Temporary - DEBUGGING
            activeWeatherSet = weatherSets[0];


            weatherLastFrame = GetWeather();
            WeatherEvent currentWeatherEvent = WeatherEventFromWeatherType(weatherLastFrame);
            currentWeatherEvent.OnActivate();
        }

        /// <summary>
        /// Get the weather using the default tracked transform for position and, in procedural mode, time since level load as time
        /// </summary>
        /// <returns>The weather at the position</returns>
        private WeatherTypes GetWeather()
        {
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    Vector2 position = new Vector2(weatherQueryLocation.position.x, weatherQueryLocation.position.z); // x and z becase player moves laterally in the x/z plane
                    return GetWeatherProcedural(position, timeExtension.CheckedTimeSinceLevelLoad);
                case WeatherMode.Manual:
                    return manualEventsSequence[eventSequenceIndex].weatherEvent.WeatherType;
                default:
                    Debug.LogError("Unknown WeatherMode - " + procedural.ToString());
                    return WeatherTypes.None;
            }
        }
        
        /// <summary>
        /// Get the weather for a particular place and the current instance
        /// </summary>
        /// <param name="weatherQueryLocation">The position at which to get the weather</param>
        public WeatherTypes GetWeather(Vector2 weatherQueryLocation)
        {
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    return GetWeatherProcedural(weatherQueryLocation, timeExtension.CheckedTimeSinceLevelLoadNoUpdate, false); //Don't update tracked value as this can be called externally and we only want to updated tracked values once per frame
                case WeatherMode.Manual:
                    return GetWeatherManual(false); //as with procedural, don't update tracked values from external calls
                default:
                    Debug.LogError("Unknown WeatherMode - " + procedural);
                    return WeatherTypes.None;
            }
        }

        private WeatherTypes GetWeatherManual(bool updateTrackedValues = true)
        {
            if (updateTrackedValues)
            {
                KeyValuePair<HumidityVariables, TemperatureVariables> humityTemperaturePair;
                proceduralWeatherLookup.LookupTable.TryReverseLookup(manualEventsSequence[eventSequenceIndex].weatherEvent.WeatherType, out humityTemperaturePair);
                if (humityTemperaturePair.Equals(default(KeyValuePair<HumidityVariables, TemperatureVariables>)))
                {
                    Debug.LogError("Cannot reverse lookup weather event (" + manualEventsSequence[eventSequenceIndex].weatherEvent.name + "). Ensure weather lookup property is assigned correctly. temperature and humidity values will be incorrect.");
                }
                else
                {
                    humidityLastFrame = humityTemperaturePair.Key;
                    temperatureLastFrame = humityTemperaturePair.Value;
                }
            }

            return manualEventsSequence[eventSequenceIndex].weatherEvent.WeatherType;
        }

        /// <summary>
        /// Get the weather for a particular time and place
        /// </summary>
        /// <param name="weatherQueryLocation">The position at which to get the weather</param>
        /// <param name="time">The time at which the weather should be queried</param>
        /// <returns>The WeatherTypes for that position at that time and updates the the tracked X and Y values</returns>
        private WeatherTypes GetWeatherProcedural(Vector2 weatherQueryLocation, float time, bool updateTrackedValues = true)
        {
            TemperatureVariables temperature = GetTemperatureValueAt(weatherQueryLocation).ToTemperatureValue();
            HumidityVariables humidity = GetHumidityValueAt(weatherQueryLocation).ToHumidityValue();

            Vector2 wind = GetWindValueAt(weatherQueryLocation);

            if (updateTrackedValues)
            {
                temperatureLastFrame = temperature;
                humidityLastFrame = humidity;
                
                trackedX += wind.x;
                trackedY += wind.y;
            }

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

        /// <summary>
        /// Get the wind direction at this instance for the given position
        /// </summary>
        /// <param name="position">The position to query the wind at</param>
        /// <returns>the wind at this specific time at the given position</returns>
        public Vector2 GetWindValueAt(Vector2 position)
        {
            Vector2 wind = new Vector2();
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    float timeScale = 0.1f; //To stop weather changes happening too quickly, we scale the time
                    wind = Generators.GetDirectionalNoise(position.x, position.y, worldSize.x, worldSize.y, proceduralScale, timeExtension.CheckedTimeSinceLevelLoadNoUpdate * timeScale);
                    wind *= windStrength;
                    break;
                case WeatherMode.Manual:
                    wind = Vector2.zero; //Fixed value for now, could be more intelligent
                    break;
            }
            return wind;
        }

        /// <summary>
        /// Get the tracked, cumulative wind for the duration
        /// </summary>
        /// <returns>The cumulative wind at the weatherQueryLocation</returns>
        public Vector2 GetCumulativeWind()
        {
            return new Vector2(trackedX, trackedY);
        }

        public float GetTemperatureValueAt(Vector2 position)
        {
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    return Generators.GetTemperatureValue(position.x + trackedX, position.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f);
                case WeatherMode.Manual:
                    return 0.0f; //TODO: - calculate from temperatureLastFrame
                default:
                    Debug.LogError("Unknown mode - " + procedural);
                    throw new System.NotImplementedException("Unknown mode - " + procedural);
            }
        }

        public float GetHumidityValueAt(Vector2 position)
        {
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    return Generators.GetHumidityValue(position.x + trackedX, position.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f);
                case WeatherMode.Manual:
                    return 0.0f; //TODO: - calculate from humidityLastFrame
                default:
                    Debug.LogError("Unknown mode - " + procedural);
                    throw new System.NotImplementedException("Unknown mode - " + procedural);
            }
        }

        public float GetIntensityValueAt(Vector2 position)
        {
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    return Generators.GetIntensityNoise(position.x + trackedX, position.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f);
                case WeatherMode.Manual:
                    float timeCompleteProportion = timeSinceSequenceChange / manualEventsSequence[eventSequenceIndex].time;
                    float newIntensity = manualEventsSequence[eventSequenceIndex].intensityOverTime.Evaluate(timeCompleteProportion);
                    return newIntensity;
            }
            throw new System.Exception("No handler for intensity in " + procedural + " mode");
        }

        // Update is called once per frame
        void Update()
        {
            switch (procedural)
            {
                case WeatherMode.Procedural:
                    ProceduralUpdate();
                    break;
                case WeatherMode.Manual:
                    ManualUpdate();
                    break;
                default:
                    Debug.LogWarning("Unknown weather mode, " + procedural.ToString());
                    break;
            }
        }

        private void ProceduralUpdate()
        {
            if (transitionCoroutine == null) //when no weather transition occuring
            {
                WeatherTypes currentWeather = GetWeather();
                WeatherEvent currentWeatherEvent = WeatherEventFromWeatherType(weatherLastFrame);
                if (currentWeather != weatherLastFrame) //If the weather changed this frame
                {
                    WeatherEvent newWeatherEvent = WeatherEventFromWeatherType(currentWeather);
                    if (currentWeatherEvent == null)
                    {
                        Debug.LogError("No weather event set for " + weatherLastFrame);
                        return;
                    }
                    if (newWeatherEvent == null)
                    {
                        Debug.LogError("No weather event set for " + currentWeather);
                        return;
                    }
                    transitionCoroutine = StartCoroutine(Transition(currentWeatherEvent, newWeatherEvent, weatherEventTransitionTime));
                    weatherLastFrame = currentWeather;
                }
                else //No weather changed, update intensity
                {
                    float intensity = GetIntensityValueAt(weatherQueryLocation.position); //Generators.GetIntensityNoise(weatherQueryLocation.position.x + trackedX, weatherQueryLocation.position.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f);
                    Vector2 wind = GetWindValueAt(weatherQueryLocation.position);
                    currentWeatherEvent.IntensityData = new IntensityData(intensity, temperatureLastFrame, humidityLastFrame, wind, currentWeather);
                    intensityPlot.AddKey(new Keyframe(timeExtension.CheckedTimeSinceLevelLoad, currentWeatherEvent.IntensityData.intensity));
                }
            }
        }

        private void ManualUpdate()
        {
            if (transitionCoroutine == null)
            {
                float newIntensity = GetIntensityValueAt(Vector2.zero); //position irrelevant in manual mode
                manualEventsSequence[eventSequenceIndex].weatherEvent.IntensityData = new IntensityData(newIntensity, temperatureLastFrame, humidityLastFrame, Vector2.zero, manualEventsSequence[eventSequenceIndex].weatherEvent.WeatherType); //wind fixed
                //Debugging
                intensityPlot.AddKey(Time.timeSinceLevelLoad, newIntensity);


                if (timeSinceSequenceChange > manualEventsSequence[eventSequenceIndex].time) //Time to transition
                {
                    int nextSequenceIndex = eventSequenceIndex + 1;
                    if (nextSequenceIndex >= manualEventsSequence.Count)
                    {
                        nextSequenceIndex = 0;
                    }

                    int oldIndex = eventSequenceIndex;
                    eventSequenceIndex = nextSequenceIndex;
                    GetWeatherManual(); //Calling this updates the lastFrame values for temp and humidity based off the current weather event

                    transitionCoroutine = StartCoroutine(Transition(manualEventsSequence[oldIndex].weatherEvent, manualEventsSequence[eventSequenceIndex].weatherEvent, manualEventsSequence[oldIndex].transitionTime));
                    timeSinceSequenceChange = 0.0f;
                }
                else
                {
                    timeSinceSequenceChange += Time.deltaTime;
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
        private IEnumerator Transition(WeatherEvent currentWeatherEvent, WeatherEvent nextWeatherEvent, float transitionTime)
        {
            float startIntensity = currentWeatherEvent.IntensityData.intensity;
            Vector2 startWind = currentWeatherEvent.IntensityData.wind;

            if(OnWeatherChangeBeginEvent != null)
            {
                OnWeatherChangeBeginEvent(new WeatherChangeEventArgs(nextWeatherEvent, currentWeatherEvent));
            }

            nextWeatherEvent.OnActivate();
            float evaluationValue = 0.0f;
            while(evaluationValue <= transitionTime)
            {
                float stepVal = transitionCurve.Evaluate(evaluationValue/transitionTime);

                float newIntensity = Mathf.Lerp(startIntensity, 0.0f, stepVal);
                Vector2 newWind = Vector2.Lerp(startWind, Vector2.zero, stepVal);

                if (evaluationValue < 0.5f) //first half of transition
                {
                    nextWeatherEvent.IntensityData = new IntensityData(1.0f - newIntensity, temperatureLastFrame, humidityLastFrame, newWind, nextWeatherEvent.WeatherType);
                    currentWeatherEvent.IntensityData = new IntensityData(newIntensity, temperatureLastFrame, humidityLastFrame, newWind, currentWeatherEvent.WeatherType);
                }
                else //second half of transition
                {
                    currentWeatherEvent.IntensityData = new IntensityData(newIntensity, temperatureLastFrame, humidityLastFrame, newWind, currentWeatherEvent.WeatherType);
                    nextWeatherEvent.IntensityData = new IntensityData(1.0f - newIntensity, temperatureLastFrame, humidityLastFrame, newWind, nextWeatherEvent.WeatherType);
                }

                //Debugging
                intensityPlot.AddKey(timeExtension.CheckedTimeSinceLevelLoadNoUpdate, currentWeatherEvent.Intensity);
                intensityPlot.AddKey(timeExtension.CheckedTimeSinceLevelLoadNoUpdate + 0.001f, nextWeatherEvent.Intensity);

                if(OnWeatherChangeStep != null)
                {
                    OnWeatherChangeStep(new WeatherChangeEventArgs(nextWeatherEvent, currentWeatherEvent));
                }

                yield return null;
                evaluationValue += Time.deltaTime;
            }
            //currentWeatherEvent.OnDeactivate();

            //re-call OnActivate for the new weather in case there were crossover WeatherProperties down the chain
            nextWeatherEvent.OnActivate();

            if (OnWeatherChangeCompleteEvent != null)
            {
                OnWeatherChangeCompleteEvent(new WeatherChangeEventArgs(nextWeatherEvent, currentWeatherEvent));
            }

            transitionCoroutine = null;
        }
	}
}