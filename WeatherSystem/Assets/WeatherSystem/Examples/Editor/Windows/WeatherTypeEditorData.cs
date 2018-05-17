using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.Inspectors
{
    [System.Serializable]
    public class WeatherTypeEditorData : ScriptableObject
    {
        [SerializeField]
        public string[] enumEntries; //The working copy
        [SerializeField]
        public string[] storedEntries; //The entries saved to disk

        public WeatherTypeEditorData(string[] storedEntries)
        {
            this.enumEntries = storedEntries;
            this.storedEntries = storedEntries;
        }
    }
}