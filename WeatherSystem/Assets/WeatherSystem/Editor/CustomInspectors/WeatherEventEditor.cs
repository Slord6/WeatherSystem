using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

namespace WeatherSystem.Inspectors
{
    [CustomEditor(typeof(WeatherEvent))]
	public class WeatherEventEditor : Editor
	{

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            WeatherEvent weatherEvent = (WeatherEvent)target;

            FieldInfo propertiesField = typeof(WeatherEvent).GetField("customProperties", BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertiesField == null)
            {
                throw new ArgumentException("WeatherEvent has no customProperties field");
            }
            DrawField(propertiesField, serializedObject); //draw default property field for customProperties from the serialized property

            //if there is an assigned weather properties, grab it
            WeatherProperties weatherProperties = (WeatherProperties)propertiesField.GetValue(weatherEvent);

            if (weatherProperties != null)
            {
                FieldInfo nonRealiantPropertiesField = typeof(WeatherProperties).GetField("weatherProperties", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo realiantPropertiesField = typeof(WeatherProperties).GetField("reliantWeatherProperties", BindingFlags.Instance | BindingFlags.NonPublic);
                WeatherProperty[] weatherPropertyArray = (WeatherProperty[])nonRealiantPropertiesField.GetValue(weatherProperties);
                ReliantWeatherProperty[] reliantWeatherPropertyArray = (ReliantWeatherProperty[])realiantPropertiesField.GetValue(weatherProperties);

                FieldInfo curvesField = typeof(WeatherEvent).GetField("curves", BindingFlags.Instance | BindingFlags.NonPublic);
                AnimationCurve[] curves = (AnimationCurve[])curvesField.GetValue(weatherEvent);

                int curvesLength = weatherPropertyArray.Length + reliantWeatherPropertyArray.Length;
                if (curves == null)
                {
                    curves = new AnimationCurve[curvesLength];
                    
                    for (int i = 0; i < curves.Length; i++)
                    {
                        curves[i] = GetDefaultCurve();
                    }
                }

                //need to lengthen or shorten array (or fix a null element)
                if (curves.Length != curvesLength || curves.Contains(null))
                {
                    AnimationCurve[] newCurves = new AnimationCurve[curvesLength];
                    if (curves.Length < curvesLength)
                    {
                        for (int i = 0; i < curvesLength; i++)
                        {
                            if (i < curves.Length && curves[i] != null)
                            {
                                newCurves[i] = curves[i];
                            }
                            else
                            {
                                newCurves[i] = GetDefaultCurve();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < curvesLength; i++)
                        {
                            if (curves[i] == null)
                            {
                                newCurves[i] = GetDefaultCurve();
                            }
                            else
                            {
                                newCurves[i] = curves[i];
                            }
                        }
                    }
                    curves = newCurves;
                }
                
                GUILayout.Label("");
                curves = DrawWeatherPropertyCurveArray(weatherPropertyArray, curves, 0, "No weather properties");
                GUILayout.Label("");
                curves = DrawWeatherPropertyCurveArray(reliantWeatherPropertyArray, curves, weatherPropertyArray.Length, "No reliant weather properties");

                //Save curves back to the object
                curvesField.SetValue(weatherEvent, curves);
            }
            else
            {
                GUILayout.Label("No assigned weather properties, assign one to edit intensity curves");
            }
            EditorUtility.SetDirty(weatherEvent);
            serializedObject.ApplyModifiedProperties();
        }

        private AnimationCurve GetDefaultCurve()
        {
            //Linear curve - 0,0->1,1;
            return AnimationCurve.Linear(0, 0, 1, 1);
        }

        private AnimationCurve[] DrawWeatherPropertyCurveArray(WeatherProperty[] weatherPropertyArray, AnimationCurve[] curves, int curvesStartIndex, string errLabel)
        {
            GUILayout.BeginVertical();
            if (weatherPropertyArray != null)
            {
                for(int i = 0; i < weatherPropertyArray.Length; i++)
                {
                    WeatherProperty weatherProperty = weatherPropertyArray[i];

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(weatherProperty.name + ": ");
                    Rect rect = EditorGUILayout.GetControlRect();
                    curves[i + curvesStartIndex] = EditorGUI.CurveField(rect, curves[i + curvesStartIndex]);
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.Label(errLabel);
            }
            GUILayout.EndVertical();

            return curves;
        }

        /// <summary>
        /// Using the serilizedObject, extract serialized properties using the given field info and draw to inspector
        /// </summary>
        /// <param name="fields">The fields to draw</param>
        private void DrawField(FieldInfo fieldInfo, SerializedObject serializedObject)
        {
            SerializedProperty property = serializedObject.FindProperty(fieldInfo.Name);
            if (property == null)
            {
                Debug.LogError("DrawFields passed an invalid field and cannot draw, skipping '" + fieldInfo.Name + "'."
                     + " Has the field got the attribute '[SerializeField]'?");
            }
            UnityEditor.EditorGUILayout.PropertyField(property, true);
        }
    }
}