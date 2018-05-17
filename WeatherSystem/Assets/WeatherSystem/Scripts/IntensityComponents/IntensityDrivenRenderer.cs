using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// An intensity driven system for changing the float value on a material attached to a specific renderer
    /// </summary>
	public class IntensityDrivenRenderer : IntensityDrivenBehaviour
	{
        //https://forum.unity.com/threads/how-to-change-normal-map-intensity-on-the-new-standard-shader-via-script-at-runtime.300184/

        [SerializeField]
        private string materialValueName;
        [SerializeField]
        private new Renderer renderer;

        private float initialValue;

        protected override void ActivationBehaviour()
        {
            initialValue = renderer.material.GetFloat(materialValueName);
        }

        protected override void FadeDelegate(float t)
        {
            renderer.material.SetFloat(materialValueName, initialValue);
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            renderer.material.SetFloat(materialValueName, intensityData.intensity);
        }
    }
}