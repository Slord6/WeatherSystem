using UnityEngine;
using System.Collections;

namespace WeatherSystem.IntensityComponents
{
	public class IntensityDrivenFog : IntensityDrivenBehaviour
	{
        [SerializeField]
        private float fogStartScale = 10.0f;
        [SerializeField]
        private float fogDensityScale = 0.1f;
        [SerializeField]
        private float fogLength = 40.0f;

        private FogInfo fogInfo;

        public override void OnActivate()
        {
            fogInfo = new FogInfo();
            RenderSettings.fog = true;
            base.OnActivate();
        }

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            RenderSettings.fogDensity = intensityData.intensity * fogDensityScale;

            RenderSettings.fogStartDistance = intensityData.intensity * fogStartScale;
            RenderSettings.fogEndDistance = RenderSettings.fogStartDistance + fogLength;
        }

        public override void OnDeactivate()
        {
            fogInfo.AssignValuesToRenderSettings();
            base.OnDeactivate();
        }

        /// <summary>
        /// Helper class for managing Fog RenderSettings
        /// </summary>
        private class FogInfo
        {
            public float fogDensity;
            public float fogStart;
            public float fogEnd;
            public bool fogActive;

            /// <summary>
            /// Assigns fog data based on RenderSettings values
            /// </summary>
            public FogInfo()
            {
                this.fogActive = RenderSettings.fog;
                this.fogDensity = RenderSettings.fogDensity;
                this.fogStart = RenderSettings.fogStartDistance;
                this.fogEnd = RenderSettings.fogEndDistance;
            }

            /// <summary>
            /// Asign the stored fog values to RenderSettings
            /// </summary>
            public void AssignValuesToRenderSettings()
            {
                RenderSettings.fog = fogActive;
                RenderSettings.fogDensity = this.fogDensity;
                RenderSettings.fogStartDistance = this.fogStart;
                RenderSettings.fogEndDistance = this.fogEnd;
            }
        }
    }
}