using System;
using UnityEditor;
using UnityEngine;

namespace UnityPlayerPrefsX.Settings
{
    public class PlayerPrefsXSettingsEditorWindow : EditorWindow
    {
        PlayerPrefsXSettingsSO settings;

        [MenuItem("Tools/PlayerPrefsX/Open Settings", priority = 20)]
        public static void Open()
        {
            // dock next to the inspector
            var inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            GetWindow<PlayerPrefsXSettingsEditorWindow>("PlayerPrefsX Settings", inspectorType);
        }

        [MenuItem("Tools/PlayerPrefsX/Clear All PlayerPrefsX", priority = 40)]
        public static void ClearAllPlayerPrefsX()
        {
            if (EditorUtility.DisplayDialog(
                "Clear All PlayerPrefsX",
                "Are you sure you want to clear all PlayerPrefsX? This will also clear all regular" +
                    " PlayerPrefs.",
                "Yes",
                "No"
            ))
            {
                PlayerPrefsX.DeleteAll();
            }
        }

        void OnEnable()
        {
            settings = PlayerPrefsXSettingsSO.Instance;

            if (!settings)
            {
                PlayerPrefsXSettingsSO.CreateSettingsAsset();
                settings = PlayerPrefsXSettingsSO.Instance;
            }
        }

        void OnGUI()
        {
            if (!settings)
            {
                Debug.LogWarning("Settings asset not found. It should have been created automatically.");
                Close();
                return;
            }

            EditorGUILayout.LabelField("PlayerPrefsX Settings", EditorStyles.boldLabel);
            settings.ErrorLogType = (ErrorLogType)EditorGUILayout.EnumPopup("Error Log Type",
                settings.ErrorLogType);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}