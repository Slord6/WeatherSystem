using UnityEngine;
using WeatherSystem.InstanceEvents;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// A chance event driven by the intensity value of intensity data
    /// </summary>
	public class IntensityDrivenInstanceEvent : WeatherTypeSpecificIntensityDrivenBehaviour
    {
        [SerializeField]
        [Range(0.0f, 0.25f)] //between 0% and 25%
        private float instanceChance = 0.1f; //default of 10%
        [SerializeField]
        private InstanceEvent instanceEvent;

        protected override void ActivationBehaviour()
        {
            base.ActivationBehaviour();
        }

        protected override void FadeDelegate(float t)
        {
            instanceEvent.FadeDelegate(t);
        }

        /// <summary>
        /// Utilises the intensity element of the IntensityData to caluclate a chance for the event to occur, if a threshold is met, the event is activated
        /// </summary>
        /// <param name="intensityData">The intensity data</param>
        protected override void ConditionalUpdateWithIntensity(IntensityData intensityData)
        {
            //as intensity increases, chance of occurence also increases
            float x = transform.position.x * intensityData.wind.x + Time.timeSinceLevelLoad * 100f;
            float y = transform.position.z * intensityData.wind.y + Time.timeSinceLevelLoad * 100f;
            float randomNumber = WeatherSystem.Internal.Generators.GetPerlinNoise(x, y, 500, 500, 1, 0);

            if (randomNumber < instanceChance)
            {
                instanceEvent.Activate(intensityData);
            }
        }
    }
}