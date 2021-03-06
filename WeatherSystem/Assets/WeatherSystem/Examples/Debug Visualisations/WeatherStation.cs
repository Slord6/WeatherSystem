using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeatherSystem;

public class WeatherStation : MonoBehaviour {

    [SerializeField]
    private WeatherManager weatherManager;
    [SerializeField]
    private Text outputText;
    [SerializeField]
    private Renderer selfRenderer;

    private WeatherSystem.Internal.TimeExtension time;

    void Start()
    {
        if(weatherManager == null)
        {
            weatherManager = FindObjectOfType<WeatherManager>();
        }
        time = FindObjectOfType<WeatherSystem.Internal.TimeExtension>();
    }

	// Update is called once per frame
	void Update ()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        WeatherTypes weather = weatherManager.GetWeather(position);
        outputText.text = weather.ToString();
        selfRenderer.material.color = weather.ToColor();
	}
}
