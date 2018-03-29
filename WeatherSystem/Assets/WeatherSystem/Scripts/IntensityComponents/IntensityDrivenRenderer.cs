using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenRenderer : IntensityDrivenBehaviour
	{
        //https://forum.unity.com/threads/how-to-change-normal-map-intensity-on-the-new-standard-shader-via-script-at-runtime.300184/

        [SerializeField]
        private string materialValueName;
        [SerializeField]
        private new Renderer renderer;

        private float initialValue;

        public override void OnActivate()
        {
            initialValue = renderer.material.GetFloat(materialValueName);
        }

        public override void OnDeactivate()
        {
            renderer.material.SetFloat(materialValueName, initialValue);
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            renderer.material.SetFloat(materialValueName, intensityData.intensity);
        }
    }
}