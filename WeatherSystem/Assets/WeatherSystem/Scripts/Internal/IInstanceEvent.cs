using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// An interface for an object that can be treated as an instance event
    /// </summary>
    public interface IInstanceEvent
    {
        /// <summary>
        /// A method for gradualy reducing the effect of the object over multiple frames
        /// </summary>
        /// <param name="t"></param>
        void FadeDelegate(float t);
        /// <summary>
        /// Some activation behaviour
        /// </summary>
        /// <param name="data">Intensity data</param>
        void Activate(IntensityData data);

    }
}
