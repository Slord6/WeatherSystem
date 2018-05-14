using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.Internal
{
    public interface IInstanceEvent
    {
        void FadeDelegate(float t);
        void Activate(IntensityData data);

    }
}
