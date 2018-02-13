using UnityEngine;
using System.Collections.Generic;
using WeatherSystem.Internal;

namespace WeatherSystem
{
    /// <summary>
    /// Acts as a wrapper to a double dictionary of type WeatherVariables,WeatherVariables,WeatherTypes
    /// This lets us have a nice inspector and a scriptable object, so we can have multiple setups
    /// </summary>
    [CreateAssetMenu(menuName = "Weather System/Procedural Weather Lookup")]
    [SerializeField]
	public class ProceduralWeatherLookup : ScriptableObject, ISerializationCallbackReceiver
    {
        //Not serialized
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

        public DoubleDictionary<HumidityVariables, TemperatureVariables, WeatherTypes> LookupTable
        {
            get
            {
                return internalLookup;
            }
        }

        //Serialized
        [SerializeField]
        private List<HumidityVariables> primaryKeys = new List<HumidityVariables>();
        [SerializeField]
        private List<TemperatureVariables> secondaryKeys = new List<TemperatureVariables>();
        [SerializeField]
        private List<WeatherTypes> values = new List<WeatherTypes>();
        
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


        public void OnAfterDeserialize()
        {
            //Unity has just written new data into the serialized fields.
            //let's populate our actual runtime data with those new values.
            internalLookup = new DoubleDictionary<HumidityVariables, TemperatureVariables, WeatherTypes>();
            for (int i = 0; i < primaryKeys.Count; i++)
            {
                internalLookup.Add(primaryKeys[i], secondaryKeys[i], values[i]);
            }
        }
    
    }
}