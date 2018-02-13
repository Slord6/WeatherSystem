using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEditor;
using System.IO;

namespace WeatherSystem
{
    public class WeatherTypesWindow : EditorWindow
    {
        [SerializeField]
        public string[] enumEntries; //The working copy
        [SerializeField]
        public string[] storedEntries; //The entries saved to disk
        private bool wasValidLastCheck = true;
        private string errorMessage = "?";

        private static WeatherTypesWindow window;

        static void Init()
        {
            window = (EditorWindow.GetWindow<WeatherTypesWindow>());
            window.storedEntries = System.Enum.GetNames(typeof(WeatherTypes));
            window.enumEntries = window.storedEntries;
        }

        [MenuItem("WeatherSystem/Weather Types")]
        public static void ShowWindow()
        {
            //EditorWindow.GetWindow(typeof(WeatherTypesWindow));
            Init();
            window.Show();
        }

        void OnGUI()
        {
            // The actual window code goes here
            //errorMessage = "";
            GUILayout.Label("Types of Weather:", EditorStyles.boldLabel);
            SerializedObject serializedObject = new SerializedObject(this);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enumEntries"), true);

            //List all weather types at the end
            if (enumEntries != null && enumEntries.Length > 0)
            {
                string enumsString = "";
                if (enumEntries.Length > 0)
                {
                    enumsString = enumEntries.Aggregate((current, next) => current + ", " + next);
                }
                else
                {
                    enumsString = "None";
                }
                GUILayout.Label("Current: " + enumsString);

                if (storedEntries.Length > 0)
                {
                    enumsString = storedEntries.Aggregate((current, next) => current + ", " + next);
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

            return null;
        }

        void OnValidate()
        {
            //Ensure no duplicates in enum list
            for(int i = 0; i < enumEntries.Length; i++)
            {
                for (int j = 0; j < enumEntries.Length; j++)
                {
                    string errorI = GetEnumError(enumEntries[i]);
                    string errorJ = GetEnumError(enumEntries[j]);
                    if (errorI == null && errorJ == null)
                    {
                        //Don't compare elements to themselves
                        if (i == j)
                        {
                            continue;
                        }
                        else
                        {

                            if (enumEntries[i] == enumEntries[j])
                            {
                                errorMessage = "Duplicate entries are not allowed";
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
            if(enumEntries == null || storedEntries == null)
            {
                Debug.LogWarning("Cannot check if save is required as instance variables are null (WeatherTypesWindow)");
                return false;
            }

            if (enumEntries.Length != storedEntries.Length)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < enumEntries.Length; i++)
                {
                    if (enumEntries[i] != storedEntries[i])
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
                for (int i = 0; i < enumEntries.Length; i++)
                {
                    streamWriter.WriteLine("\t" + enumEntries[i] + ",");
                }
                streamWriter.WriteLine("}");
            }
            storedEntries = enumEntries;
            AssetDatabase.Refresh();
        }
    }
}