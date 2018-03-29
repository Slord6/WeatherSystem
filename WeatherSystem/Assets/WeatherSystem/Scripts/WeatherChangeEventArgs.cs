using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem
{
    public class WeatherChangeEventArgs
    {
        private WeatherEvent enteringWeatherEvent;
        private WeatherEvent exitingWeatherEvent;

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
