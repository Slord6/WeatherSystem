using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeatherSystem.InstanceEvents
{
    public class InstanceLightEvent : InstanceEvent
    {

        [SerializeField]
        private Light light;
        [SerializeField]
        private int maxFlashes = 4;
        [SerializeField]
        [Range(0.001f, 0.1f)]
        private float maxtimePerFlash = 0.01f;
        [SerializeField]
        private float timeBetweenFlashes = 0.01f;

        private Coroutine flashCoroutine;

        public override void Activate(IntensityData data)
        {
            if(flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            
            int flashCount = (int)Mathf.Max(1, maxFlashes * data.intensity); //min 1 flash, up to max no of flashes
            float[] flashTimes = new float[flashCount];

            for(int i = 0; i < flashCount; i++)
            {
                flashTimes[i] = Random.Range(0f, maxtimePerFlash);
            }

            flashCoroutine = StartCoroutine(Flash(flashTimes));
        }

        private IEnumerator Flash(float[] flashTimes)
        {
            for(int i = 0; i < flashTimes.Length; i++)
            {
                light.enabled = true;
                yield return new WaitForSeconds(flashTimes[i]);
                light.enabled = false;
                yield return new WaitForSeconds(timeBetweenFlashes);
            }

            light.enabled = false;
            flashCoroutine = null;
        }

        public override void FadeDelegate(float t)
        {
            light.enabled = false;
        }
    }
}