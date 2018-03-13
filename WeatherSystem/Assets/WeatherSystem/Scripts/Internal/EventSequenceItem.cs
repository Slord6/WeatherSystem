using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WeatherSystem.Internal
{
    [Serializable]
    public class EventSequenceItem
    {
        public WeatherEvent weatherEvent;
        public float time;
        public float transitionTime;
        public AnimationCurve intensityOverTime;
        
    }
}