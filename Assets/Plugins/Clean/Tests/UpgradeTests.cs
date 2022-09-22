using System;
using System.IO;
using NUnit.Framework;
using Plugins.Clean.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Tests
{
    public class UpgradeTests
    {
        

        private static Text CreateText(string text)
        {
            var go = new GameObject();
            var textComponent = go.AddComponent<Text>();
            textComponent.text = text;
            return textComponent;
        }
        
        private static GameObject CreateTextPrefab(string text, string path)
        {
            var t = CreateText(text);
            var prefab = PrefabUtility.SaveAsPrefabAsset(t.gameObject, path, out var hasSaved);
            if (!hasSaved) throw new Exception("Could not create prefab");

            return prefab;
        }

        private static string assetPath = "Assets/tmp~";
        [OneTimeSetUp]
        public void CreateTmpDir()
        {
            if (!Directory.Exists(assetPath))
                AssetDatabase.CreateFolder("Assets", "tmp~");
        }

        [Test]
        public void CanUpgradeTextComponent()
        {
            var testText = "some text";
            var textComponent = CreateText(testText);
            var go = textComponent.gameObject;

            Util.UpgradeText(textComponent);

            var tmp = go.GetComponent<TextMeshProUGUI>();
            Assert.IsNotNull(tmp, "Failed to add TextMeshProUGUI component");
            Assert.IsNull(tmp.GetComponent<Text>(), "Failed to remove Text component");
            Assert.AreEqual(tmp.text, testText, "Failed to update text field");
        }

        [OneTimeTearDown]
        public void RemoveTempDir()
        {
            if (Directory.Exists(assetPath))
                FileUtil.DeleteFileOrDirectory(assetPath);
            if(File.Exists($"{assetPath}.meta"))
                FileUtil.DeleteFileOrDirectory($"{assetPath}.meta");
                
        }
    }
}