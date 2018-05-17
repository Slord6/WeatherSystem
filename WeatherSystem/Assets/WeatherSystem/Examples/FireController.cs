using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherSystem;
using WeatherSystem.IntensityComponents;
using System.Linq;

/// <summary>
/// Monobehaviour-derived component which controlls a fire's particle effect based on the weather at the fire's position
/// </summary>
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
        //Convert position to 2D for the queries to the weather system
        Vector2 position = new Vector2(transform.position.x, transform.position.z);

        //Get the current weather event and its associated properties
        WeatherEvent weatherEvent = weatherManager.GetWeatherEventAt(position);
        WeatherProperties properties = weatherEvent.Properties;

        float? intensity = properties.EvaluateIntensityData(this.PropertyParent, weatherEvent.WeatherPropertiesIntensityCurves, weatherManager.GetIntensityValueAt(position));
        if(intensity == null)
        {
            Debug.LogError("Intensity value could not be found");
            return;
        }

        //This is driven by precipitation, so we want the inverse intensity of that
        float fireIntensity = 1f - (float)intensity;

        //Change look of fire based on the intensity
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
