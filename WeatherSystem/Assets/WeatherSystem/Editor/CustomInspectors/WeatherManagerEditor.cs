using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

namespace WeatherSystem
{
    [CustomEditor(typeof(WeatherManager))]
	public class WeatherManagerEditor : Editor
	{
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //Cast target to WeatherManager and get all fields
            WeatherManager weatherSystem = (WeatherManager)target;
            FieldInfo[] objectFields = typeof(WeatherManager).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            ///Organise fields by attriubte
            List<FieldInfo> proceduralFields = new List<FieldInfo>();
            List<FieldInfo> manualFields = new List<FieldInfo>();
            List<FieldInfo> serializedFields = new List<FieldInfo>();
            objectFields.ToList().ForEach((field) =>
            {
                ProceduralAttribute proceduralAttribute = (ProceduralAttribute)Attribute.GetCustomAttribute(field, typeof(ProceduralAttribute));
                if (proceduralAttribute != null)
                {
                    proceduralFields.Add(field);
                    return;
                }

                ManualAttribute manualAttribute = (ManualAttribute)Attribute.GetCustomAttribute(field, typeof(ManualAttribute));
                if (manualAttribute != null)
                {
                    manualFields.Add(field);
                    return;
                }

                SerializeField serializeAttribute = (SerializeField)Attribute.GetCustomAttribute(field, typeof(SerializeField));
                if(serializeAttribute != null)
                {
                    serializedFields.Add(field);
                    return;
                }
            });
            objectFields = null;
            
            //First, draw all the non-specific fields
            List<string> nonDefaultFields = new List<string>();
            nonDefaultFields.AddRange(manualFields.Select(field =>
            {
                return field.Name;
            }));
            nonDefaultFields.AddRange(proceduralFields.Select(field =>
            {
                return field.Name;
            }));
            DrawPropertiesExcluding(serializedObject, nonDefaultFields.ToArray());

            //Now draw the elements relevent to weatherSystem.procedural value
            //The procedural variables if selected and manual ones otherwise
            weatherSystem.procedural = ToggleElements(weatherSystem.procedural, "Procedural Mode?", proceduralFields, manualFields);

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws a toggle and displays a set of fields depending on the value of the toggle
        /// </summary>
        /// <param name="toggleSelect">The current toggle state</param>
        /// <param name="toggleText">The name of the toggle</param>
        /// <param name="trueToggleElements">The fields to be drawn if the toggle is true</param>
        /// <param name="falseToggleElements">The fields to be drawn if the toggle is false</param>
        /// <returns>The update toggle value</returns>
        private bool ToggleElements(bool toggleSelect, string toggleText, List<FieldInfo> trueToggleElements, List<FieldInfo> falseToggleElements)
        {
            //Then draw a toggle for the mode select and store the result of the toggle
            toggleSelect = GUILayout.Toggle(toggleSelect, toggleText);
            
            if (toggleSelect)
            {
                DrawFields(trueToggleElements);
            }
            else
            {
                DrawFields(falseToggleElements);
            }

            return toggleSelect;
        }

        /// <summary>
        /// Using the serilizedObject, extract serialized properties using the given field info and draw to inspector
        /// </summary>
        /// <param name="fields">The fields to draw</param>
        private void DrawFields(List<FieldInfo> fields)
        {
            foreach (FieldInfo fieldInfo in fields)
            {
                SerializedProperty property = serializedObject.FindProperty(fieldInfo.Name);
                if(property == null)
                {
                    Debug.LogError("DrawFields passed an invalid field and cannot draw, skipping '" + fieldInfo.Name + "'");
                }
                UnityEditor.EditorGUILayout.PropertyField(property, true);
            }
        }
    }
}