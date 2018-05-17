using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem
{
    /// <summary>
    /// Data relating to the change from one weather event to another
    /// </summary>
    public class WeatherChangeEventArgs
    {
        /// <summary>
        /// The weather event the system is activating/activated/will activate
        /// </summary>
        public WeatherEvent enteringWeatherEvent;
        /// <summary>
        /// The weather event the system is deactivating/deactivated/will deactivate
        /// </summary>
        public WeatherEvent exitingWeatherEvent;

        public WeatherChangeEventArgs(WeatherEvent enteringWeatherEvent, WeatherEvent exitingWeatherEvent)
        {
            this.enteringWeatherEvent = enteringWeatherEvent;
            this.exitingWeatherEvent = exitingWeatherEvent;
        }

        public override string ToString()
        {
            return "Exit from " + exitingWeatherEvent.name + " to " + enteringWeatherEvent.name;
        }
    }
}
