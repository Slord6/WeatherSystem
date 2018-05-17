using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// A scriptable object that has associated intensity data
    /// </summary>
	public class IntensityScriptableObject : ScriptableObject, IIntensityDriven
	{
        [SerializeField]
        [Range(0.0f,1.0f)]
        private IntensityData intensityData;

        /// <summary>
        /// The intensity data of this weather event at this instance
        /// </summary>
        public virtual IntensityData IntensityData
        {
            get
            {
                return intensityData;
            }
            set
            {
                intensityData = value;
                if (intensityData.intensity > 1)
                {
                    intensityData.intensity = 1;
                }
                else if (intensityData.intensity < 0)
                {
                    intensityData.intensity = 0;
                }
            }
        }

        /// <summary>
        /// Shortcut for IntensityData.intensity
        /// </summary>
        public float Intensity
        {
            get
            {
                return intensityData.intensity;
            }
        }
    }
}