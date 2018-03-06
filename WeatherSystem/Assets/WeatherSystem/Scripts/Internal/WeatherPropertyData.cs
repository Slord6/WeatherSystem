using UnityEngine;
using System.Collections;

namespace WeatherSystem.Internal
{
	public class WeatherPropertyData
    {
        public float rawIntensity;

        public float windIntensity;
        public float precipitationIntensity;
        public float debrisIntensity;
        public float cloudIntensity;
        public float lightIntensity;
        public float backgroundSoundIntensity;
        public float instanceSoundIntensity;
        public Color lightColor;
        public WeatherProperty customProperties;
        
        public WeatherPropertyData()
        {

        }

        public WeatherPropertyData LerpTo(WeatherPropertyData otherData, float t)
        {
            WeatherPropertyData newData = new WeatherPropertyData();

            newData.rawIntensity = Mathf.Lerp(this.rawIntensity, otherData.rawIntensity, t);
            newData.windIntensity = Mathf.Lerp(this.windIntensity, otherData.windIntensity, t);
            newData.precipitationIntensity = Mathf.Lerp(this.precipitationIntensity, otherData.precipitationIntensity, t);
            newData.debrisIntensity = Mathf.Lerp(this.debrisIntensity, otherData.debrisIntensity, t);
            newData.lightIntensity = Mathf.Lerp(this.lightIntensity, otherData.lightIntensity, t);
            newData.cloudIntensity = Mathf.Lerp(this.cloudIntensity, otherData.cloudIntensity, t);
            newData.backgroundSoundIntensity = Mathf.Lerp(this.backgroundSoundIntensity, otherData.backgroundSoundIntensity, t);
            newData.instanceSoundIntensity = Mathf.Lerp(this.instanceSoundIntensity, otherData.instanceSoundIntensity, t);
            newData.lightColor = Color.Lerp(this.lightColor, otherData.lightColor, t);

            if(t < 0.5f)
            {
                newData.customProperties = this.customProperties;
            }
            else
            {
                newData.customProperties = otherData.customProperties;
            }

            return newData;
        }

    }
}