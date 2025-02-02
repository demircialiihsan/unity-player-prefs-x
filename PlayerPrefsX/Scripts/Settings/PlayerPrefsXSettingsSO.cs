using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityPlayerPrefsX.Settings
{
    public class PlayerPrefsXSettingsSO : ScriptableObject
    {
        const string assetName = "PlayerPrefsXSettings";

        public ErrorLogType ErrorLogType
        {
            get => errorLogType;
            set => errorLogType = value;
        }
        [SerializeField] ErrorLogType errorLogType = ErrorLogType.Error;

        static PlayerPrefsXSettingsSO instance;

        public static PlayerPrefsXSettingsSO Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<PlayerPrefsXSettingsSO>(assetName);             
                return instance;
            }
        }

#if UNITY_EDITOR
        public static void CreateSettingsAsset()
        {
            var settings = CreateInstance<PlayerPrefsXSettingsSO>();

            var resourcesPath = "Assets/Resources";

            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            var path = $"{resourcesPath}/{assetName}.asset";

            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{assetName} asset is created in Resources folder.");
        }
#endif
    }
}