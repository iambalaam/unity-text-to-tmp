using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Clean.Tests
{
    public static class Helpers
    {
        public static string SetupTmpDir()
        {
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists($"Assets/{guid}")) ;
            {
                AssetDatabase.CreateFolder("Assets", guid);
            }
            return guid;
        }

        public static void CleanupTmpDir(string guid)
        {
            if (Directory.Exists($"Assets/{guid}")) ;
            FileUtil.DeleteFileOrDirectory($"Assets/{guid}");
            if (File.Exists($"Assets/{guid}.meta"))
                FileUtil.DeleteFileOrDirectory($"Assets/{guid}.meta");
        }

        public static Text CreateText(string text)
        {
            var go = new GameObject();
            var textComponent = go.AddComponent<Text>();
            textComponent.text = text;
            return textComponent;
        }

        public static GameObject CreateTextPrefab(string text, string path)
        {
            GameObject prefab = null;
            var t = CreateText(text);
            prefab = PrefabUtility.SaveAsPrefabAsset(t.gameObject, path, out var hasSaved);
            if (!hasSaved) throw new Exception("Could not create prefab");
            if (prefab == null) throw new Exception("Could not return prefab");

            return prefab;
        }
    }
}