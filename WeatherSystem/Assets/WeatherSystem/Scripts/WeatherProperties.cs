using UnityEngine;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Weather Properties")]
    public class WeatherProperties : ScriptableObject
	{
        [SerializeField]
        private WeatherProperty[] weatherProperties;

        [SerializeField]
        private ReliantWeatherProperty[] reliantWeatherProperties;

        public void ApplyPropertyData(WeatherPropertyData data)
        {
            //apply changes to non reliant
            foreach (WeatherProperty weatherProperty in weatherProperties)
            {
                weatherProperty.ApplyPropertyData(data);
            }

            //then notify reliant to update
        }
    }
}