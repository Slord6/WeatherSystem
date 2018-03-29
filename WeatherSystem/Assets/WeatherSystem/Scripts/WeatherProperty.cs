using UnityEngine;
using WeatherSystem.IntensityComponents;
using System.Collections.Generic;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Weather Property")]
    public class WeatherProperty : ScriptableObject, IActivatable
	{
        [Header("Assign a weather property to an IntensityDrivenBehaviour to have the behaviour updated at runtime")]
        private List<IntensityDrivenBehaviour> drivenComponents;
        
        public virtual void OnActivate()
        {
            if (drivenComponents == null || drivenComponents.Contains(null) || drivenComponents.Count == 0)
            {
                drivenComponents = new List<IntensityDrivenBehaviour>();
                IntensityDrivenBehaviour[] activeIntensityBehaviours = FindObjectsOfType<IntensityDrivenBehaviour>();

                for (int i = 0; i < activeIntensityBehaviours.Length; i++)
                {
                    if (activeIntensityBehaviours[i].PropertyParent == this)
                    {
                        drivenComponents.Add(activeIntensityBehaviours[i]);
                    }
                }
            }

            drivenComponents.ForEach(element => element.OnActivate());
        }

        public virtual void OnDeactivate()
        {
            drivenComponents.ForEach(element => element.OnDeactivate());
            drivenComponents = null;
        }

        /// <summary>
        /// Iterates over all IntensityDrivenBehaviours held by this object and updates the intensity of each
        /// </summary>
        /// <param name="intensity">The new intensity to apply</param>
        public virtual void ApplyIntensity(IntensityData intensityData)
        {
            foreach(IntensityDrivenBehaviour intensityBehaviour in drivenComponents)
            {
                intensityBehaviour.IntensityData = intensityData;
            }
        }
    }
}