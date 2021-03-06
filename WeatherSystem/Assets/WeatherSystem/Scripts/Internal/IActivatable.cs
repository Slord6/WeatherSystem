using UnityEngine;
using System.Collections;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// Interface for an object that can be activated or deactivated
    /// </summary>
	public interface IActivatable
	{
        /// <summary>
        /// Performs any tasks required for setup as the object activates
        /// </summary>
        void OnActivate();
        /// <summary>
        /// Performs any required cleanup of the object as the object deactivates
        /// </summary>
        void OnDeactivate();
    }
}