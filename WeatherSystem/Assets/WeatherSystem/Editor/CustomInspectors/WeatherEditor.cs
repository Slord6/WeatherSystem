using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor class that extends Editor, adding useful methods
/// </summary>
public class WeatherEditor : Editor
{
    /// <summary>
    /// Using the serilizedObject, extract serialized properties using the given field info and draw to inspector
    /// </summary>
    /// <param name="fields">The fields to draw</param>
    protected virtual void DrawFields(List<FieldInfo> fields)
    {
        foreach (FieldInfo fieldInfo in fields)
        {
            SerializedProperty property = serializedObject.FindProperty(fieldInfo.Name);
            if (property == null)
            {
                Debug.LogError("DrawFields passed an invalid field and cannot draw, skipping '" + fieldInfo.Name + "'."
                     + " Has the field got the attribute '[SerializeField]'?");
                continue;
            }
            UnityEditor.EditorGUILayout.PropertyField(property, true);
        }
    }
    
    /// <summary>
    /// Using the serilizedObject, extract serialized properties using the given field info and draw to inspector
    /// </summary>
    /// <param name="fields">The fields to draw</param>
    protected virtual void DrawField(FieldInfo fieldInfo, SerializedObject serializedObject)
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
