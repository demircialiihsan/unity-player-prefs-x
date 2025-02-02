# if PLAYER_PREFS_X_EDITOR

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;
using UnityEngine;

namespace UnityPlayerPrefsX
{
    public class PlayerPrefsXJsonEditorWindow : EditorWindow
    {
        string keyInput;
        string validKey;

        string jsonInput;
        Vector2 scroll;

        [MenuItem("Tools/PlayerPrefsX/Open JSON Editor", priority = 25)]
        public static void Open()
        {
            var inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            GetWindow<PlayerPrefsXJsonEditorWindow>("PlayerPrefsX Json Editor", inspectorType);
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("PlayerPrefsX JSON Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            EditorGUILayout.BeginHorizontal();

            keyInput = EditorGUILayout.TextField("Key", keyInput);

            if (string.IsNullOrEmpty(keyInput))
                GUI.enabled = false;

            if (GUILayout.Button("Search", GUILayout.Width(80)))
            {
                GetJson();
                GUI.FocusControl(null);
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            if (!string.IsNullOrEmpty(validKey))
            {
                EditorGUILayout.BeginHorizontal(new GUIStyle("box"));

                EditorGUILayout.LabelField(validKey);

                if (GUILayout.Button("Update", GUILayout.MinWidth(80)))
                {
                    SaveJson();
                    GUI.FocusControl(null);
                }
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    validKey = null;
                }

                EditorGUILayout.EndHorizontal();

                scroll = EditorGUILayout.BeginScrollView(scroll);

                var lineCount = jsonInput.Split('\n').Length;
                jsonInput = EditorGUILayout.TextArea(jsonInput, GUILayout.Height(lineCount * 15 + 10));

                EditorGUILayout.EndScrollView();
            }
        }

        void GetJson()
        {
            if (!PlayerPrefs.HasKey(keyInput))
            {
                EditorUtility.DisplayDialog(
                    "Key Not Found",
                    $"Key: \"{keyInput}\" does not exist in PlayerPrefs.",
                    "OK"
                );
                return;
            }

            try
            {
                var json = PlayerPrefs.GetString(keyInput);
                jsonInput = JToken.Parse(json).ToString(Formatting.Indented);
                validKey = keyInput;
            }
            catch
            {
                EditorUtility.DisplayDialog(
                    "Error",
                    $"Value of key: \"{keyInput}\" is not a valid JSON.",
                    "OK"
                );
            }
        }

        void SaveJson()
        {
            try
            {
                var json = JToken.Parse(jsonInput).ToString(Formatting.None);
                PlayerPrefs.SetString(validKey, json);
                EditorUtility.DisplayDialog("Update Complete", "PlayerPrefs updated successfully.", "OK");
            }
            catch
            {
                EditorUtility.DisplayDialog("Update Failed", "Invalid JSON format. Cannot update.", "OK");
            }
        }
    }
}

#endif