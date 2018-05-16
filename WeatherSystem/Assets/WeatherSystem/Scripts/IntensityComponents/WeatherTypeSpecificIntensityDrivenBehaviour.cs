using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeatherSystem.IntensityComponents
{
    public class WeatherTypeSpecificIntensityDrivenBehaviour : ConditionalIntensityDrivenComponent
    {
        [SerializeField]
        private WeatherTypes[] validWeatherTypes;

        protected override bool ShouldUpdate(IntensityData intensityData)
        {
            return validWeatherTypes.Contains(intensityData.weatherType);
        }
    }
}
