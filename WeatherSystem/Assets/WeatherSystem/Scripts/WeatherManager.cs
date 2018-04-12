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


            if (updateTrackedValues)
            {
                temperatureLastFrame = temperature;
                humidityLastFrame = humidity;


                float timeScale = 0.1f; //To stop weather changes happening too quickly, we scale the time

                Vector2 wind = Generators.GetDirectionalNoise(weatherQueryLocation.x, weatherQueryLocation.y, worldSize.x, worldSize.y, proceduralScale, time * timeScale);
                wind *= windStrength;
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
                    float intensity = Generators.GetIntensityNoise(weatherQueryLocation.position.x + trackedX, weatherQueryLocation.position.y + trackedY, worldSize.x, worldSize.y, proceduralScale, 0.00f);
                    currentWeatherEvent.IntensityData = new IntensityData(intensity, temperatureLastFrame, humidityLastFrame);
                    intensityPlot.AddKey(new Keyframe(timeExtension.CheckedTimeSinceLevelLoad, currentWeatherEvent.IntensityData.intensity));
                }
            }
        }

        private void ManualUpdate()
        {
            if (transitionCoroutine == null)
            {
                timeSinceSequenceChange += Time.deltaTime;
                if (timeSinceSequenceChange > manualEventsSequence[eventSequenceIndex].time)
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
                    //Set the intensity, moving along the curve proportionally with the time we spend in this weather event
                    float newIntensity = manualEventsSequence[eventSequenceIndex].intensityOverTime.Evaluate(((float)timeSinceSequenceChange / manualEventsSequence[eventSequenceIndex].transitionTime));
                    manualEventsSequence[eventSequenceIndex].weatherEvent.IntensityData = new IntensityData(newIntensity, temperatureLastFrame, humidityLastFrame); 
                    //Debugging
                    intensityPlot.AddKey(Time.timeSinceLevelLoad, manualEventsSequence[eventSequenceIndex].weatherEvent.IntensityData.intensity);
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

                if (evaluationValue < 0.5f) //first half of transition
                {
                    nextWeatherEvent.IntensityData = new IntensityData(1.0f - newIntensity, temperatureLastFrame, humidityLastFrame);
                    currentWeatherEvent.IntensityData = new IntensityData(newIntensity, temperatureLastFrame, humidityLastFrame);
                }
                else //second half of transition
                {
                    currentWeatherEvent.IntensityData = new IntensityData(newIntensity, temperatureLastFrame, humidityLastFrame);
                    nextWeatherEvent.IntensityData = new IntensityData(1.0f - newIntensity, temperatureLastFrame, humidityLastFrame);
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