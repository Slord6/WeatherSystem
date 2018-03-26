using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.Internal
{
    public class TimeExtension : MonoBehaviour
    {
        /// <summary>
        /// Used to update the time values only once per frame
        /// </summary>
        private bool checkedThisFrame = false;

        /// <summary>
        /// The cumulative Time.DeltaTime in every frame the time has been queried
        /// </summary>
        private float checkedTimeSinceLevelLoad = 0.0f;

        /// <summary>
        /// The cumulative Time.DeltaTime in every frame the time has been queried.
        /// The first call to this in a given frame will trigger the update of all time values.
        /// </summary>
        public float CheckedTimeSinceLevelLoad
        {
            get
            {
                DoFrameCheck();
                return checkedTimeSinceLevelLoad;
            }
        }

        /// <summary>
        /// The cumulative Time.DeltaTime in every frame the time has been queried.
        /// Will not update the stored time value.
        /// </summary>
        public float CheckedTimeSinceLevelLoadNoUpdate
        {
            get
            {
                return checkedTimeSinceLevelLoad;
            }
        }

        /// <summary>
        /// Checks if the times have been updated this frame and updates them if not
        /// </summary>
        private void DoFrameCheck()
        {
            if (!checkedThisFrame)
            {
                checkedThisFrame = true;

                //Update all tracked time values here
                checkedTimeSinceLevelLoad += Time.deltaTime;
            }
        }

        /// <summary>
        /// This is the last game object call, so if the script order is set correctly
        /// (this goes last) then time will update correctly on frames where the time is queried
        /// </summary>
        private void LateUpdate()
        {
            checkedThisFrame = false;
        }
    }
}