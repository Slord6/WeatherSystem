using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Reliant Weather Property")]
    public class ReliantWeatherProperty : WeatherProperty
	{
        [Header("Parent-based intensity settings")]
        [SerializeField]
        private RelianceWeighting[] intensityParentWeightings;

        public new float Intensity
        {
            get
            {
                return base.Intensity;
            }
            set
            {
                //Iterate through the intensityParentWeightings to calculate the new intensity and apply it
                float total = 0.0f;
                for (int i = 0; i < intensityParentWeightings.Length; i++)
                {
                    float intensityValue = intensityParentWeightings[i].intensityParent.Intensity;
                    intensityValue = intensityParentWeightings[i].weightingCurve.Evaluate(intensityValue);
                    total += intensityValue;
                }
                total /= intensityParentWeightings.Length;
                //Defer to parent implementation now that we've caluclated the intensity based on the parents'
                base.Intensity = total;
            }
        }
	}
}