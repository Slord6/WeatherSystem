using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WeatherSystem;

namespace WeatherSystem
{
	public class WeatherManager : MonoBehaviour
	{
        [HideInInspector]
        public bool procedural = false;

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