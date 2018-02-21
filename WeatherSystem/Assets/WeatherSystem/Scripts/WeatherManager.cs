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

        //Fields with the attribute "Manual" are only drawn when
        //procedural is false
        [SerializeField]
        [Manual]
        private List<WeatherSet> weatherSets;

        //Fields with the attribute "Manual" are only drawn when
        //procedural is true
        [SerializeField]
        [Procedural]
        private List<WeatherEvent> weatherEvents;
        [SerializeField]
        [Procedural]
        private ProceduralWeatherLookup proceduralWeatherLookup;

		// Use this for initialization
		protected virtual void Start ()
		{
		
		}
		
		// Update is called once per frame
		void Update ()
		{
		
		}
	}
}