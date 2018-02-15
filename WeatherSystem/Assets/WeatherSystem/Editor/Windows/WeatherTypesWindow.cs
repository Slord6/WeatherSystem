using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace WeatherSystem.Inspectors
{
    public class WeatherTypesWindow : EditorWindow
    {
        private static WeatherTypeEditorData data;
        private bool wasValidLastCheck = true;
        private string errorMessage = "?";
        
        static void Init()
        {
            string[] storedEntries = System.Enum.GetNames(typeof(WeatherTypes));
            //Remove the default "None"
            List<string> temp = storedEntries.ToList();
            temp.Remove("None");
            data = ScriptableObject.CreateInstance<WeatherTypeEditorData>();
            data.enumEntries = temp.ToArray();
            data.storedEntries = temp.ToArray();
        }

        [MenuItem("WeatherSystem/Weather Types")]
        public static void ShowWindow()
        {
            Init();
            EditorWindow.GetWindow<WeatherTypesWindow>();
        }

        void OnGUI()
        {
            // The actual window code goes here
            GUILayout.Label("Types of Weather:", EditorStyles.boldLabel);

            //After a save (and after unity recompiles the code), the reference to data is sometimes lost
            //So if we lose it...
            if(data == null)
            {
                //...we'll re-initalise
                Init();
            }

            SerializedObject serializedObject = new SerializedObject(data);
            
            SerializedProperty currentEntries = serializedObject.FindProperty("enumEntries");
            //Display the enums
            EditorGUILayout.PropertyField(currentEntries, true);
            
            //List all weather types at the end
            if (data.enumEntries != null && data.enumEntries.Length > 0)
            {
                string enumsString = "";
                if (data.enumEntries.Length > 0)
                {
                    enumsString = "None, " + data.enumEntries.Aggregate((current, next) => current + ", " + next);
                }
                else
                {
                    enumsString = "None";
                }
                GUILayout.Label("Current: " + enumsString);

                if (data.storedEntries.Length > 0)
                {
                    enumsString = "None, " + data.storedEntries.Aggregate((current, next) => current + ", " + next);
                }
                else
                {
                    enumsString = "None";
                }
                GUILayout.Label("Stored: " + enumsString);
            }
            else
            {
                GUILayout.Label("No weather types defined");
            }
            
            GUIStyle redStyle = new GUIStyle();
            redStyle.normal.textColor = Color.red;
            if (SaveRequired())
            {
                if (wasValidLastCheck)
                {
                    GUILayout.Label("Not Saved!", redStyle);
                    if (GUILayout.Button("Save"))
                    {
                        SaveEnums();
                    }
                }
                else
                {
                    GUILayout.Label("Save disabled: " + errorMessage, redStyle);
                }
            }
            else
            {
                GUIStyle greenStyle = new GUIStyle();
                greenStyle.normal.textColor = Color.blue;
                GUILayout.Label("Save up to date", greenStyle);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private string GetEnumError(string enumString)
        {
            if (string.IsNullOrEmpty(enumString))
            {
                return "Element cannot be empty";
            }
            if (enumString.Contains(" ") || enumString.Contains('\t'))
            {
                return "Element cannot contain whitespace";
            }
            if(enumString == "None")
            {
                return "None is a default value";
            }

            return null;
        }

        void OnValidate()
        {
            //After a save (and after unity recompiles the code), the reference to data is sometimes lost
            //if there's nothing to check...
            if (data == null)
            {
                //...then just return
                return;
            }


            //Ensure no duplicates in enum list
            for (int i = 0; i < data.enumEntries.Length; i++)
            {
                for (int j = 0; j < data.enumEntries.Length; j++)
                {
                    string errorI = GetEnumError(data.enumEntries[i]);
                    string errorJ = GetEnumError(data.enumEntries[j]);
                    if (errorI == null && errorJ == null)
                    {
                        //Don't compare elements to themselves
                        if (i == j)
                        {
                            continue;
                        }
                        else
                        {

                            if (data.enumEntries[i] == data.enumEntries[j])
                            {
                                errorMessage = "Duplicate entries are not allowed (" + i + ", " + j + ")";
                                wasValidLastCheck = false;
                                return;
                            }

                        }
                    }
                    else
                    {
                        if(errorI != null)
                        {
                            errorMessage = errorI + " ("+ i + ")";
                        }
                        else
                        {
                            errorMessage = errorJ + " (" + j + ")";
                        }
                        wasValidLastCheck = false;
                        return;
                    }
                }
            }
            wasValidLastCheck = true;
        }

        private bool SaveRequired()
        {
            if(data.enumEntries == null || data.storedEntries == null)
            {
                Debug.LogWarning("Cannot check if save is required as instance variables are null (WeatherTypesWindow)");
                return false;
            }

            if (data.enumEntries.Length != data.storedEntries.Length)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < data.enumEntries.Length; i++)
                {
                    if (data.enumEntries[i] != data.storedEntries[i])
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        
        private void SaveEnums()
        {
            MonoScript ms = MonoScript.FromScriptableObject(this);
            string m_ScriptFilePath = AssetDatabase.GetAssetPath(ms);

            FileInfo fileInfo = new FileInfo(m_ScriptFilePath);
            string m_ScriptFolder = fileInfo.Directory.ToString();
            m_ScriptFolder.Replace('\\', '/');
            
            DirectoryInfo parentDir = Directory.GetParent(m_ScriptFolder);
            while (parentDir.FullName.Contains("Editor"))
            {
                parentDir = Directory.GetParent(parentDir.FullName);
            }
            
            string enumName = "WeatherTypes";
            string filePathAndName = parentDir.FullName + @"\Scripts\DynamicEnums\";
            StreamWriter streamWriter;
            if (!Directory.Exists(filePathAndName))
            {
                Directory.CreateDirectory(filePathAndName);
            }
            filePathAndName += "WeatherTypes.cs";

            if (!File.Exists(filePathAndName))
            {
                streamWriter = File.CreateText(filePathAndName);
            }
            else
            {
                streamWriter = new StreamWriter(filePathAndName);
            }

            using (streamWriter)
            {
                streamWriter.WriteLine("/// <summary>");
                streamWriter.WriteLine("/// DO NOT EDIT");
                streamWriter.WriteLine("/// This enum is automatically generated from WeatherSystem->WeatherTypes in the editor");
                streamWriter.WriteLine("/// </summary>");

                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("\t" + "None" + ",");
                for (int i = 0; i < data.enumEntries.Length; i++)
                {
                    streamWriter.WriteLine("\t" + data.enumEntries[i] + ",");
                }
                streamWriter.WriteLine("}");
            }
            data.storedEntries = data.enumEntries;
            AssetDatabase.Refresh();
        }
    }
}