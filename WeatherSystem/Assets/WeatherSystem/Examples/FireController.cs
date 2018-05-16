using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherSystem;
using WeatherSystem.IntensityComponents;
using System.Linq;

public class FireController : IntensityDrivenBehaviour
{
    [SerializeField]
    private ParticleSystem fireParticleEffect;
    [SerializeField]
    private WeatherManager weatherManager;

    protected override void ActivationBehaviour()
    {
        if(weatherManager == null)
        {
            weatherManager = FindObjectOfType<WeatherManager>();
        }
    }

    protected void Update()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        WeatherEvent weatherEvent = weatherManager.GetWeatherEventAt(position);
        WeatherProperties properties = weatherEvent.Properties;

        float? intensity = properties.EvaluateIntensityData(this.PropertyParent, weatherEvent.WeatherPropertiesIntensityCurves, weatherManager.GetIntensityValueAt(position));
        if(intensity == null)
        {
            Debug.LogError("Intensity value could not be found");
            return;
        }

        float fireIntensity = 1f - (float)intensity;

        ParticleSystem.EmissionModule emission = fireParticleEffect.emission;
        emission.rateOverTime = fireIntensity * 10;

        ParticleSystem.MainModule mainModule = fireParticleEffect.main;
        mainModule.startLifetime = fireIntensity * 5f;
    }

    protected override void UpdateWithIntensity(IntensityData intensityData)
    {
        //Do nothing with fed values, as those are at the player's location
    }
}
