using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherSystem.Internal;

namespace WeatherSystem.InstanceEvents
{
    /// <summary>
    /// A base implementation of IInstance interface as a Monobehaviour
    /// </summary>
    public class InstanceEvent : MonoBehaviour, IInstanceEvent
    {
        public virtual void Activate(IntensityData data)
        {
        }

        public virtual void FadeDelegate(float t)
        {
        }
    }
}