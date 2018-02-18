using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		
	}
}