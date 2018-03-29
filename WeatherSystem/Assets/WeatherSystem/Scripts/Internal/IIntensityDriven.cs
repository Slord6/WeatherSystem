using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherSystem.Internal
{
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
