using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

namespace WeatherSystem.Inspectors
{
    [CustomEditor(typeof(WeatherManager))]
	public class WeatherManagerEditor : WeatherEditor
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
            //Do we need this? The returns in the .ForEach loop above must mean we don't?
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
            weatherSystem.procedural = (WeatherMode)SwitchElements(weatherSystem.procedural, "Procedural Mode?", new List<List<FieldInfo>> { proceduralFields, manualFields });
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Uses user selection of an enum value to select a set of fields to draw
        /// </summary>
        /// <param name="enumValue">The current enum value</param>
        /// <param name="switchText">The text to display to the user</param>
        /// <param name="elementsSelection">The selection of lists of elements. This must be indexed to match the integer value of the provided enum</param>
        /// <returns>The user-selected enum value</returns>
        private Enum SwitchElements(Enum enumValue, string switchText, List<List<FieldInfo>> elementsSelection)
        {
            enumValue = EditorGUILayout.EnumPopup(enumValue);
            int index = Convert.ToInt32(enumValue);
            DrawFields(elementsSelection[index]);
            return enumValue;
        }

        /// <summary>
        /// Draws a toggle and displays a set of fields depending on the value of the toggle
        /// </summary>
        /// <param name="toggleSelect">The current toggle state</param>
        /// <param name="toggleText">The name of the toggle</param>
        /// <param name="trueToggleElements">The fields to be drawn if the toggle is true</param>
        /// <param name="falseToggleElements">The fields to be drawn if the toggle is false</param>
        /// <returns>The updated toggle value</returns>
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
    }
}