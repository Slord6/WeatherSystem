using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System.Collections.Generic;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Properties/Weather Property")]
    public class WeatherProperty : ScriptableObject
	{
        [Header("Audio")]
        [SerializeField]
        private AudioClip backgroundSound;
        [SerializeField]
        private AudioClip[] instanceSounds;
        [SerializeField]
        [Range(0, 1.0f)]
        private float instanceSoundChance = 0.5f;
        [SerializeField]
        private AudioSource backgroundSoundsAudioSource;
        [SerializeField]
        private AudioSource instanceSoundsAudioSource;

        [Header("Visuals")]
        [SerializeField]
        private ParticleSystem precipitationParticleSystem;
        [SerializeField]
        private Shader precipitationShader; //Shader needs to be some inherited version with an intensity value to set
        [SerializeField]
        private Light worldLight;
        [SerializeField]
        private Color lightColor;

        [Header("Custom Value")]
        [Tooltip("Objects with components that inherit from IIntensityDriven, which will be passed a raw intensity value")]
        [SerializeField]
        private GameObject[] affectedObjects;

        private List<IIntensityDriven> intensityElements = null;
        
        public WeatherPropertyData LastAssignedPropertyData { get; set; }

        public virtual void ApplyPropertyData(WeatherPropertyData data)
        {
            LastAssignedPropertyData = data;

            //Apply to the default systems
            SetBackgroundSounds(data.backgroundSoundIntensity);

            SetInstanceSounds(data.instanceSoundIntensity);

            SetPrecipitation(data.precipitationIntensity);

            SetLight(data.lightIntensity, data.lightColor);

            SetCustomObjects(data.rawIntensity);

            //Then access custom data and apply to that
        }

        protected virtual void SetBackgroundSounds(float intensity)
        {
            if (backgroundSoundsAudioSource != null)
            {
                backgroundSoundsAudioSource.volume = intensity;
            }
        }

        protected virtual void SetInstanceSounds(float intensity)
        {
            if (instanceSounds != null && instanceSounds != null)
            {
                float instanceSoundChance = UnityEngine.Random.Range(0, intensity);
                if (instanceSoundChance < this.instanceSoundChance)
                {
                    int index = UnityEngine.Random.Range(0, instanceSounds.Length);
                    instanceSoundsAudioSource.loop = false;
                    instanceSoundsAudioSource.clip = instanceSounds[index];
                    instanceSoundsAudioSource.Play();
                }
            }
        }

        protected virtual void SetPrecipitation(float intensity)
        {
            if(precipitationParticleSystem != null)
            {
                ParticleSystem.EmissionModule emission = precipitationParticleSystem.emission;
                emission.rateOverTime = intensity * 100;
            }
            if(precipitationShader != null)
            {
                //SET INTENSITY
            }
        }

        protected virtual void SetLight(float intensity, Color lightColor)
        {
            if(worldLight != null)
            {
                worldLight.intensity = intensity;
                worldLight.color = lightColor;
            }
        }
        
        private void SetCustomObjects(float rawIntensity)
        {
            if(intensityElements == null)
            {
                intensityElements = new List<IIntensityDriven>();
                foreach(GameObject affectedObject in affectedObjects)
                {
                    intensityElements.AddRange(affectedObject.GetComponents<IIntensityDriven>());
                }
            }

            foreach(IIntensityDriven intensityComponent in intensityElements)
            {
                intensityComponent.Intensity = rawIntensity;
            }
        }
    }
}