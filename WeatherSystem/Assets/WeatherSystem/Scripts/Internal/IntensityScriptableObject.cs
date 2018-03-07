using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;

namespace WeatherSystem.Internal
{
	public class IntensityScriptableObject : ScriptableObject, IIntensityDriven
	{
        [SerializeField]
        [Range(0.0f,1.0f)]
        private float intensity = 1.0f;

        /// <summary>
        /// The intensity of this weather event at this instance
        /// </summary>
        public virtual float Intensity
        {
            get
            {
                return intensity;
            }
            set
            {
                intensity = value;
                if (intensity > 1)
                {
                    intensity = 1;
                }
                else if (intensity < 0)
                {
                    intensity = 0;
                }
            }
        }
    }
}