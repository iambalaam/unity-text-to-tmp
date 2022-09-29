using System;
using System.IO;
using NUnit.Framework;
using Plugins.Clean.Editor;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Tests
{
    [TestFixture]
    public class ComponentUpgradeTests
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
            GameObject prefab = null;
            var t = CreateText(text);
            prefab = PrefabUtility.SaveAsPrefabAsset(t.gameObject, path, out var hasSaved);
            if (!hasSaved) throw new Exception("Could not create prefab");
            if (prefab == null) throw new Exception("Could not return prefab");

            return prefab;
        }

        public string Setup()
        {
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists($"Assets/{guid}")) ;
            {
                AssetDatabase.CreateFolder("Assets", guid);
            }
            return guid;
        }

        public void Cleanup(string guid)
        {
            if (Directory.Exists($"Assets/{guid}")) ;
            FileUtil.DeleteFileOrDirectory($"Assets/{guid}");
            if (File.Exists($"Assets/{guid}.meta"))
                FileUtil.DeleteFileOrDirectory($"Assets/{guid}.meta");
        }

        [Test]
        public void CanUpgradeTextComponent()
        {
            var testText = "some text";
            var textComponent = CreateText(testText);
            var go = textComponent.gameObject;

            ComponentUpgrade.UpgradeText(textComponent);

            var tmp = go.GetComponent<TextMeshProUGUI>();
            Assert.IsNotNull(tmp, "Failed to add TextMeshProUGUI component");
            Assert.IsNull(tmp.GetComponent<Text>(), "Failed to remove Text component");
            Assert.AreEqual(tmp.text, testText, "Failed to update text field");
        }

        [Test]
        public void CanUpgradeTextPrefab()
        {
            var guid = Setup();
            var prefabPath = $"Assets/{guid}/text.prefab";
            GameObject tmpInstance = null;

            try
            {
                // Create prefab
                CreateTextPrefab("original text", prefabPath);

                // ComponentUpgrade prefab
                ComponentUpgrade.UpgradePrefabRoot(prefabPath);

                // Instantiate clone
                tmpInstance = PrefabUtility.LoadPrefabContents(prefabPath);
                var tmp = tmpInstance.GetComponent<TextMeshProUGUI>();

                // Check clone
                Assert.IsNull(tmpInstance.GetComponent<Text>(),
                    "Failed to remove Text component from prefab instance");
                Assert.IsNotNull(tmpInstance.GetComponent<TextMeshProUGUI>(),
                    "Failed to add TextMeshProUGUI component to existing prefab instance");
                Assert.IsNotNull(tmp, "Failed to add TextMeshProUGUI component to new prefab instance");
                Assert.AreEqual(tmp.text, "original text");
            }
            finally
            {
                if (tmpInstance != null) PrefabUtility.UnloadPrefabContents(tmpInstance);
                Cleanup(guid);
            }
        }

        [Test]
        public void CanUpgradePrefabWithSceneOverrides()
        {
            var guid = Setup();
            try
            {
                // Setup scene override
                var prefab = CreateTextPrefab("original text", $"Assets/{guid}/text.prefab");
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var sceneInstance = Object.Instantiate(prefab);
                var text = sceneInstance.GetComponent<Text>();
                text.text = "scene override";
                EditorSceneManager.SaveScene(scene, $"Assets/{guid}/test-scene.unity");

                // ComponentUpgrade component
                ComponentUpgrade.UpgradeText(text);

                // Check scene override
                Assert.IsNull(sceneInstance.GetComponent<Text>(),
                    "Failed to remove Text component from prefab instance");
                Assert.IsNotNull(sceneInstance.GetComponent<TextMeshProUGUI>(),
                    "Failed to add TextMeshProUGUI component from prefab instance");
                Assert.AreEqual(sceneInstance.GetComponent<TextMeshProUGUI>().text, "scene override");

                // Check prefab
                var prefabInstance = Object.Instantiate(prefab);
                Assert.IsNull(prefabInstance.GetComponent<Text>(), "Failed to remove Text component from prefab");
                Assert.IsNotNull(prefabInstance.GetComponent<TextMeshProUGUI>(),
                    "Failed to add TextMeshProUGUI component from prefab");
                Assert.AreEqual(prefabInstance.GetComponent<TextMeshProUGUI>().text, "original text");
            }
            finally
            {
                Cleanup(guid);
            }
        }
    }
}