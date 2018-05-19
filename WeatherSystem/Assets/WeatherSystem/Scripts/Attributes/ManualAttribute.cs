using UnityEngine;
using System.Collections;
using System;

namespace WeatherSystem
{
    /// <summary>
    /// An attribute for denoting a field only used in 'Manual' mode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ManualAttribute : Attribute
    {

    }
}