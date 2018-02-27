using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherSystem;

public static class WeatherToColour
{
    public static Color ToColor(this WeatherTypes weatherType)
    {
        switch (weatherType)
        {
            case WeatherTypes.None:
                return Color.clear;
            case WeatherTypes.Clear:
                return Color.yellow;
            case WeatherTypes.Rain:
                return Color.blue;
            case WeatherTypes.Snow:
                return Color.white;
            case WeatherTypes.Overcast:
                return Color.grey;
            case WeatherTypes.Storm:
                return Color.black;
            case WeatherTypes.Hail:
                return Color.cyan;
            default:
                Debug.LogWarning("No color for " + weatherType);
                return Color.green;
        }
    }
}
