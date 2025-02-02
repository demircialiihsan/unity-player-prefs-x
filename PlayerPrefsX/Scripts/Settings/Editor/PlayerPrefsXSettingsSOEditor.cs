using UnityEditor;
using UnityEngine;

namespace UnityPlayerPrefsX.Settings
{
    [CustomEditor(typeof(PlayerPrefsXSettingsSO))]
    public class PlayerPrefsXSettingsSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var guiEnabled = GUI.enabled;

            // disable editing from the asset (editing only through settings editor window)
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = guiEnabled;
        }
    }
}