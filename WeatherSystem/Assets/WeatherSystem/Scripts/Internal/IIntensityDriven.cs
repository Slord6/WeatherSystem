using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// Interface for an object that can be driven by an IntensityData object
    /// </summary>
    interface IIntensityDriven
    {
        IntensityData IntensityData
        {
            set;
        }

        float Intensity
        {
            get;
        }
    }
}
