using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

namespace WeatherSystem.Inspectors
{
    [CustomEditor(typeof(WeatherEvent))]
	public class WeatherEventEditor : WeatherEditor
	{
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            WeatherEvent weatherEvent = (WeatherEvent)target;

            //Draw default weatherType field
            FieldInfo weatherTypeField = typeof(WeatherEvent).GetField("weatherType", BindingFlags.Instance | BindingFlags.NonPublic);
            DrawField(weatherTypeField, serializedObject);

            //Draw default customProperties field
            FieldInfo weatherPropertiesField = typeof(WeatherEvent).GetField("customProperties", BindingFlags.Instance | BindingFlags.NonPublic);
            if (weatherPropertiesField == null)
            {
                throw new ArgumentException("WeatherEvent has no customProperties field");
            }
            DrawField(weatherPropertiesField, serializedObject); //draw default property field for customProperties from the serialized property
            
            //if there is an assigned weather properties, grab it
            WeatherProperties weatherProperties = (WeatherProperties)weatherPropertiesField.GetValue(weatherEvent);
            //check there is one - if there is we do curve stuff, otherwise display a label to tell user to assign one....
            if (weatherProperties != null)
            {
                FieldInfo nonRealiantPropertiesField = typeof(WeatherProperties).GetField("weatherProperties", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo realiantPropertiesField = typeof(WeatherProperties).GetField("reliantWeatherProperties", BindingFlags.Instance | BindingFlags.NonPublic);
                WeatherProperty[] weatherPropertyArray = (WeatherProperty[])nonRealiantPropertiesField.GetValue(weatherProperties);
                ReliantWeatherProperty[] reliantWeatherPropertyArray = (ReliantWeatherProperty[])realiantPropertiesField.GetValue(weatherProperties);

                FieldInfo curvesField = typeof(WeatherEvent).GetField("curves", BindingFlags.Instance | BindingFlags.NonPublic);
                AnimationCurve[] curves = (AnimationCurve[])curvesField.GetValue(weatherEvent);

                int curvesLength = weatherPropertyArray.Length + reliantWeatherPropertyArray.Length;
                if (curves == null) //first time with this object, need to add new curves for editing
                {
                    curves = new AnimationCurve[curvesLength];
                    
                    for (int i = 0; i < curves.Length; i++)
                    {
                        curves[i] = GetDefaultCurve();
                    }
                }

                //need to lengthen or shorten array (or fix a null element)
                if (curves.Length != curvesLength || curves.Contains(null)) //number of properties has changed etc
                {
                    AnimationCurve[] newCurves = new AnimationCurve[curvesLength];
                    if (curves.Length < curvesLength) //less curves than expected...
                    {
                        for (int i = 0; i < curvesLength; i++)
                        {
                            if (i < curves.Length && curves[i] != null) //so fill the curves we have (and replace null elements)
                            {
                                newCurves[i] = curves[i];
                            }
                            else //for the rest, create a new curve
                            {
                                newCurves[i] = GetDefaultCurve();
                            }
                        }
                    }
                    else //null elements or longer than expected
                    {
                        for (int i = 0; i < curvesLength; i++)
                        {
                            if (curves[i] == null) //replace null elements with defaults
                            {
                                newCurves[i] = GetDefaultCurve();
                            }
                            else //fill the rest with the current curves
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
            //Linear curve - (0,0)->(1,1);
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

        
    }
}