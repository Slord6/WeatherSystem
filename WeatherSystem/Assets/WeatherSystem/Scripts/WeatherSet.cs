using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeatherSystem
{
    /// <summary>
    /// A scriptable object for managing WeatherEvents
    /// </summary>
    [CreateAssetMenu(menuName ="Weather System/Weather Set")]
	public class WeatherSet : ScriptableObject
    {

        [SerializeField]
        private WeatherEvent[] weatherEvents;

        public WeatherEvent[] WeatherEvents
        {
            get
            {
                return weatherEvents;
            }
        }

        public List<WeatherEvent> WeatherEventsList
        {
            get
            {
                return weatherEvents.ToList();
            }
        }
		
	}
}