using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using WeatherSystem;

public class WeatherLogger : MonoBehaviour {

    [SerializeField]
    private Transform trackedTransform;

    [SerializeField]
    private WeatherManager weatherManager;

    [Header("Logging data")]
    [SerializeField]
    private string loggingFolder;

    [SerializeField]
    private float logTime;

    private float timeSinceLog = 0f;

    private void Awake()
    {
        if (weatherManager == null)
        {
            weatherManager = FindObjectOfType<WeatherManager>();
        }

        string[] headings = { "time", "weatherType", "humEnum", "hum", "tempEnum", "temp", "intensity", "windInstance", "windTracked" };
        AddToLog(string.Format("{0}{1}", string.Join(",", headings), System.Environment.NewLine));
    }

    private void Start()
    {
        LogWeatherData();
    }

    /// <summary>
    /// Updates the text display
    /// </summary>
    private void Update()
    {
        timeSinceLog += Time.deltaTime;

        if (timeSinceLog < logTime) return;
        
        LogWeatherData();
        timeSinceLog = timeSinceLog - logTime; //carry over the remainder time (min = 0f)
    }

    private void LogWeatherData()
    {
        Vector2 windInstance, windTracked;
        float intensity;
        float temp;
        float hum;
        WeatherTypes weatherType;

        Vector2 location = new Vector2(trackedTransform.position.x, trackedTransform.position.z);

        weatherType = weatherManager.GetWeather(location);

        windInstance = weatherManager.GetWindValueAt(location);
        windTracked = weatherManager.GetCumulativeWind();

        intensity = weatherManager.GetIntensityValueAt(location);

        temp = weatherManager.GetTemperatureValueAt(location);
        TemperatureVariables tempEnum = temp.ToTemperatureValue();

        hum = weatherManager.GetHumidityValueAt(location);
        HumidityVariables humEnum = hum.ToHumidityValue();

        float time = Time.timeSinceLevelLoad;
        var data = new object[] {time, weatherType, humEnum, hum, tempEnum, temp, intensity, windInstance, windTracked};
        AddToLog(string.Format("{0}{1}", string.Join(",", (from value in data select value.ToString()).ToArray()),
            System.Environment.NewLine));
    }
        
    private void AddToLog(string text)
    {
        File.AppendAllText(string.Format("{0}/log.txt", loggingFolder), text);
    }
}
