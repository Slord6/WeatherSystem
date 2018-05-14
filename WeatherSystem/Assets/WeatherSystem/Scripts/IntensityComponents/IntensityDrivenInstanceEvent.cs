using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using WeatherSystem.InstanceEvents;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenInstanceEvent : IntensityDrivenBehaviour
    {
        [SerializeField]
        [Range(0.0f, 0.1f)] //between 0% and 10%
        private float instanceChance = 0.0005f; //default of 1/500th %
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

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            //as intensity increases, chance of occurence also increases
            float randomNumber = Random.Range(0.0f, 1.0f - intensityData.intensity);

            if (randomNumber < instanceChance)
            {
                instanceEvent.Activate(intensityData);
            }
        }
    }
}