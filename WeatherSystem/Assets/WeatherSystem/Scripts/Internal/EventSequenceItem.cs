using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// An item to be used in a sequence of WeatherEvents
    /// </summary>
    [Serializable]
    public class EventSequenceItem
    {
        /// <summary>
        /// The weather event for this point in the sequence
        /// </summary>
        public WeatherEvent weatherEvent;
        /// <summary>
        /// The time this part of the sequence should be active
        /// </summary>
        public float time;
        /// <summary>
        /// The time that should be taken to transition from this part of the sequence to the next
        /// </summary>
        public float transitionTime;
        /// <summary>
        /// The intensity of the weather event over the course of this sequence item
        /// </summary>
        public AnimationCurve intensityOverTime;
        
    }
}