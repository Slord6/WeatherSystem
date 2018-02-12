using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName ="Weather System/Weather Set")]
	public class WeatherSet : ScriptableObject
    {

        [SerializeField]
        private WeatherEvent[] weatherEvents;

		// Use this for initialization
		void Start ()
		{
		
		}
		
		// Update is called once per frame
		void Update ()
		{
		
		}
	}
}