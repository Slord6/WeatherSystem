using UnityEngine;
using System;

namespace WeatherSystem
{
    /// <summary>
    /// A data object for data relating to the reliance of an object on a WeatherProperty parent using a curve
    /// </summary>
    [Serializable]
	public class RelianceWeighting
	{
        /// <summary>
        /// The WeatherProperty parent
        /// </summary>
        public WeatherProperty intensityParent;
        /// <summary>
        /// The weighting curve for this parent
        /// </summary>
        public AnimationCurve weightingCurve;
	}
}