using UnityEngine;
using System.Collections.Generic;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    /// <summary>
    /// A 'table' of lookup values describing the weather given a humidty and temperature variable
    /// Acts as a wrapper to a DoubleDictionary of type WeatherVariables,WeatherVariables,WeatherTypes
    /// This lets us have a nice inspector and a scriptable object, so we can have multiple setups stored as assets
    /// </summary>
    [CreateAssetMenu(menuName = "Weather System/Procedural Weather Lookup")]
    [SerializeField]
	public class ProceduralWeatherLookup : ScriptableObject, ISerializationCallbackReceiver
    {
        //Not serialized as unity's serializer doesn't get along with generic collections
        [System.NonSerialized]
        private DoubleDictionary<HumidityVariables, TemperatureVariables, WeatherTypes> internalLookup = 
            new DoubleDictionary<HumidityVariables, TemperatureVariables, WeatherTypes>()
            {
                { HumidityVariables.HumidityHigh, TemperatureVariables.TemperatureLow, WeatherTypes.None },
                { HumidityVariables.HumidityHigh, TemperatureVariables.TemperatureMid, WeatherTypes.None },
                { HumidityVariables.HumidityHigh, TemperatureVariables.TemperatureHigh, WeatherTypes.None },
                { HumidityVariables.HumidityMid, TemperatureVariables.TemperatureHigh, WeatherTypes.None },
                { HumidityVariables.HumidityMid, TemperatureVariables.TemperatureMid, WeatherTypes.None },
                { HumidityVariables.HumidityMid, TemperatureVariables.TemperatureLow, WeatherTypes.None },
                { HumidityVariables.HumidityLow, TemperatureVariables.TemperatureHigh, WeatherTypes.None },
                { HumidityVariables.HumidityLow, TemperatureVariables.TemperatureMid, WeatherTypes.None },
                { HumidityVariables.HumidityLow, TemperatureVariables.TemperatureLow, WeatherTypes.None }
            };
        
        /// <summary>
        /// The internal DoubleDictionary that this object wraps
        /// </summary>
        public DoubleDictionary<HumidityVariables, TemperatureVariables, WeatherTypes> LookupTable
        {
            get
            {
                return internalLookup;
            }
        }

        //Serialized - this is used to allow unity's serialization system to handle the object
        //See below seialization callback methods
        [SerializeField]
        private List<HumidityVariables> primaryKeys = new List<HumidityVariables>();
        [SerializeField]
        private List<TemperatureVariables> secondaryKeys = new List<TemperatureVariables>();
        [SerializeField]
        private List<WeatherTypes> values = new List<WeatherTypes>();

        //Serialization reference - https://blogs.unity3d.com/2014/06/24/serialization-in-unity/

        /// <summary>
        /// Callback called prior to serialization to allow the object to transfer the internal dictionary values to lists as Unity's serialization doesn't support generic (double)dictionary objects
        /// </summary>
        public void OnBeforeSerialize()
        {
            //unity is about to read the serializedNodes field's contents. lets make sure
            //we write out the correct data into that field "just in time".
            primaryKeys.Clear();
            secondaryKeys.Clear();
            values.Clear();
            
            foreach (KeyKeyValuePair<HumidityVariables, TemperatureVariables, WeatherTypes> pair in internalLookup)
            {
                primaryKeys.Add(pair.PrimaryKey);
                secondaryKeys.Add(pair.SecondaryKey);
                values.Add(pair.Value);
            }
        }
        
        /// <summary>
        /// Callback called after deserialization of the object occurs. The now deserialized lists of dictionary values are transferred back into the disctionary
        /// </summary>
        public void OnAfterDeserialize()
        {
            //Unity has just written new data into the serialized fields.
            //let's populate our actual runtime data with those new values.
            internalLookup = new DoubleDictionary<HumidityVariables, TemperatureVariables, WeatherTypes>();
            for (int i = 0; i < primaryKeys.Count; i++)
            {
                internalLookup.Add(primaryKeys[i], secondaryKeys[i], values[i]);
            }
            //primaryKeys.Clear();
            //secondaryKeys.Clear();
            //values.Clear();
        }
    }
}