using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherSystem;

public class WeatherAnnouncer : MonoBehaviour
{
    [SerializeField]
    WeatherManager weatherManager;

    private void OnEnable()
    {
        weatherManager.OnWeatherChangeCompleteEvent += OnWeatherChange;
    }

    public void OnWeatherChange(WeatherChangeEventArgs e)
    {
        Debug.Log(e.ToString());
    }

    private void OnDisable()
    {
        weatherManager.OnWeatherChangeCompleteEvent -= OnWeatherChange;
    }
}
