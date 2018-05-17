using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WeatherSystem.Internal;
using System;

namespace WeatherSystem
{
    /// <summary>
    /// A scriptable object for managing WeatherEvents
    /// </summary>
    [CreateAssetMenu(menuName ="Weather System/Weather Set")]
	public class WeatherSet : IntensityScriptableObject
    {

        [SerializeField]
        private WeatherEvent[] weatherEvents;
        
        /// <summary>
        /// The weather events in this WeatherSet
        /// </summary>
        public WeatherEvent[] WeatherEvents
        {
            get
            {
                return weatherEvents;
            }
        }

        /// <summary>
        /// The weather events in this WeatherSet arranged into a list
        /// </summary>
        public List<WeatherEvent> WeatherEventsList
        {
            get
            {
                return weatherEvents.ToList();
            }
        }
		
	}
}