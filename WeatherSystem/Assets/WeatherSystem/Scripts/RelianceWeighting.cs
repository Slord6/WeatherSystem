using UnityEngine;
using System;

namespace WeatherSystem
{
    [Serializable]
	public class RelianceWeighting
	{
        public WeatherProperty intensityParent;
        public AnimationCurve weightingCurve;
	}
}