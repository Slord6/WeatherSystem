using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Property")]
    public class WeatherProperty : IntensityScriptableObject
	{
        [SerializeField]
        private WeatherProperty[] childProperties;

        public new float Intensity
        {
            get
            {
                return base.Intensity;
            }
            set
            {
                base.Intensity = value;
                if(childProperties != null && childProperties.Length > 0)
                {
                    for (int i = 0; i < childProperties.Length; i++)
                    {
                        childProperties[i].Intensity = value;
                    }
                }
            }
        }

    }
}