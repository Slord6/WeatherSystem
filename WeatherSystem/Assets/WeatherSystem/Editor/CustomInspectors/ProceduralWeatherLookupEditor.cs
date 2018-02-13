using UnityEngine;
using System.Collections;
using UnityEditor;
using WeatherSystem.Internal;
using System.Collections.Generic;
using System;
using System.Linq;

namespace WeatherSystem.Inspectors
{
    [CustomEditor(typeof(ProceduralWeatherLookup))]
	public class ProceduralWeatherLookupEditor : Editor
	{
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ProceduralWeatherLookup weatherLookup = (ProceduralWeatherLookup)target;

            List<HumidityVariables> humidityVariables = GetEnumValues<HumidityVariables>();
            List<TemperatureVariables> temperatureVariables = GetEnumValues<TemperatureVariables>();
            
            bool none = false; //keeps track of any WeatherTypes.None values to display a warning
            for (int i = 0; i < humidityVariables.Count; i++)
            {
                HorizontalBar();
                //Add a 'title' for this section of the current humidity enum
                GUILayout.Label(humidityVariables[i].ToString());
                GUILayout.BeginHorizontal();
                //ad each associated temperature value
                for (int j = 0; j < temperatureVariables.Count; j++)
                {
                    GUILayout.Label(temperatureVariables[j].ToString());
                }
                GUILayout.EndHorizontal();

                //Display the enum selection boxes underneath each heading
                GUILayout.BeginHorizontal();
                WeatherTypes current;
                for (int j = 0; j < temperatureVariables.Count; j++)
                {
                    if(!weatherLookup.LookupTable.TryGetValue(humidityVariables[i], temperatureVariables[j], out current))
                    {
                        current = WeatherTypes.None;
                        weatherLookup.LookupTable.Add(humidityVariables[i], temperatureVariables[j], current);
                    }
                    if(current == WeatherTypes.None)
                    {
                        none = true;
                    }
                    weatherLookup.LookupTable[humidityVariables[i], temperatureVariables[j]] = (WeatherTypes)EditorGUILayout.EnumPopup(current);
                }
                GUILayout.EndHorizontal();
            }

            //Show a warning if there are any WeatherTypes.None in the selection grid we created
            if (none)
            {
                GUIStyle redStyle = new GUIStyle();
                redStyle.normal.textColor = Color.red;
                GUILayout.Label("Some elements are set to 'none'.\r\nThis might result in strange-looking weather", redStyle);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void HorizontalBar()
        {
            EditorGUILayout.LabelField("________________________________");
            //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Looks nicer, seems to cause exceptions occasionally
        }

        public static List<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}