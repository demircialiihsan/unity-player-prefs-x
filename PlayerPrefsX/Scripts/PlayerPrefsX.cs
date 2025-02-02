using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityPlayerPrefsX.Settings;
using UnityPlayerPrefsX.Wrappers;

namespace UnityPlayerPrefsX
{
    public static class PlayerPrefsX
    {
        const ErrorLogType defaultErrorLogType = ErrorLogType.Error;

        #region public Methods

        #region PlayerPrefs Redirections

        public static bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public static void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
        public static void DeleteAll() => PlayerPrefs.DeleteAll();
        public static void Save() => PlayerPrefs.Save();

        #endregion

        public static void Set<T>(string key, T value)
        {
            if (TrySetPrimitive(key, value))
                return;

            SetJsonObject(key, value);
        }

        public static T Get<T>(string key, T defaultValue)
        {
            if (!PlayerPrefs.HasKey(key))
                return defaultValue;

            if (TryGetPrimitive(key, defaultValue, out T value))
                return value;

            try
            {
                return GetJsonObject<T>(key);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return defaultValue;
            }
        }

        public static T Get<T>(string key)
        {
            return Get<T>(key, default);
        }

        #endregion

        #region private Methods

        static bool TrySetPrimitive<T>(string key, T value)
        {
            if (value is int intValue)
            {
                PlayerPrefs.SetInt(key, intValue);
                return true;
            }
            else if (value is float floatValue)
            {
                PlayerPrefs.SetFloat(key, floatValue);
                return true;
            }
            else if (value is string stringValue)
            {
                PlayerPrefs.SetString(key, stringValue);
                return true;
            }
            else if (value is bool boolValue)
            {
                PlayerPrefs.SetInt(key, boolValue ? 1 : 0);
                return true;
            }
            return false;
        }

        static void SetJsonObject<T>(string key, T value)
        {
            string json;

            var type = typeof(T);

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var wrapperType = typeof(ArrayWrapper<>).MakeGenericType(elementType);
                var wrapper = Activator.CreateInstance(wrapperType, value);
                json = JsonUtility.ToJson(wrapper);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var wrapperType = typeof(ListWrapper<>).MakeGenericType(elementType);
                var wrapper = Activator.CreateInstance(wrapperType, value);
                json = JsonUtility.ToJson(wrapper);
            }
            else
            {
                json = JsonUtility.ToJson(value);
            }

            PlayerPrefs.SetString(key, json);
        }

        static bool TryGetPrimitive<T>(string key, T defaultValue, out T value)
        {
            var type = typeof(T);

            if (type == typeof(int))
            {
                value = (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
                return true;
            }
            else if (type == typeof(float))
            {
                value = (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
                return true;
            }
            else if (type == typeof(string))
            {
                value = (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);
                return true;
            }
            else if (type == typeof(bool))
            {
                value = (T)(object)(PlayerPrefs.GetInt(key, (bool)(object)defaultValue ? 1 : 0) != 0);
                return true;
            }

            value = default;
            return false;
        }

        static T GetJsonObject<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);

            var type = typeof(T);

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var wrapperType = typeof(ArrayWrapper<>).MakeGenericType(elementType);
                var wrapper = JsonUtility.FromJson(json, wrapperType);
                var field = wrapperType.GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
                return (T)field.GetValue(wrapper);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var wrapperType = typeof(ListWrapper<>).MakeGenericType(elementType);
                var wrapper = JsonUtility.FromJson(json, wrapperType);
                var field = wrapperType.GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
                return (T)field.GetValue(wrapper);
            }
            else
            {
                return JsonUtility.FromJson<T>(json);
            }
        }

        static void LogError(Exception exception)
        {
            var settings = PlayerPrefsXSettingsSO.Instance;
            var errorLogType = settings ? settings.ErrorLogType : defaultErrorLogType;

            switch (errorLogType)
            {
                case ErrorLogType.Warning:
                    Debug.LogWarning(exception);
                    break;
                case ErrorLogType.Error:
                    Debug.LogError(exception);
                    break;
            }
        }

        #endregion
    }
}