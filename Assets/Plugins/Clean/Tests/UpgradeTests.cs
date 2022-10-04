using NUnit.Framework;
using Plugins.Clean.Editor;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Plugins.Clean.Tests
{
    public class UpgradeTests : MonoBehaviour
    {
        [Test]
        public void CanUpgradeScenenComponent()
        {
            var guid = Helpers.SetupTmpDir();
            try
            {
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var sceneObject = new GameObject();
                var text = sceneObject.AddComponent<Text>();
                text.text = "some text";
                EditorSceneManager.SaveScene(scene, $"Assets/{guid}/test-scene.unity");

                new Upgrade().TextToTMP(new Scene[]{scene}, new string[]{});

                Assert.IsNull(sceneObject.GetComponent<Text>(),
                    "Failed to remove Text component from scene object");
                Assert.IsNotNull(sceneObject.GetComponent<TextMeshProUGUI>(),
                    "Failed to add TextMeshProUGUI component from scene object");
                Assert.AreEqual("some text", sceneObject.GetComponent<TextMeshProUGUI>().text);
            }
            finally
            {
                Helpers.CleanupTmpDir(guid);
            }
        }

        [Test]
        public void CanUpgradePrefabWithSceneOverrides()
        {
            var guid = Helpers.SetupTmpDir();
            try
            {
                // Setup scene override
                var prefabPath = $"Assets/{guid}/text.prefab";
                var prefab = Helpers.CreateTextPrefab("original text", prefabPath);
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var sceneInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                var text = sceneInstance!.GetComponent<Text>();
                text.text = "scene override";
                EditorSceneManager.SaveScene(scene, $"Assets/{guid}/test-scene.unity");

                // ComponentUpgrade component
                new Upgrade().TextToTMP(new Scene[] { scene }, new string[] { prefabPath });

                // Check scene override
                Assert.IsNull(sceneInstance.GetComponent<Text>(),
                    "Failed to remove Text component from prefab instance");
                Assert.IsNotNull(sceneInstance.GetComponent<TextMeshProUGUI>(),
                    "Failed to add TextMeshProUGUI component from prefab instance");
                Assert.AreEqual(sceneInstance.GetComponent<TextMeshProUGUI>().text, "scene override");

                // Check prefab
                var prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                Assert.IsNull(prefabInstance!.GetComponent<Text>(), "Failed to remove Text component from prefab");
                Assert.IsNotNull(prefabInstance.GetComponent<TextMeshProUGUI>(),
                    "Failed to add TextMeshProUGUI component from prefab");
                Assert.AreEqual(prefabInstance.GetComponent<TextMeshProUGUI>().text, "original text");
            }
            finally
            {
                Helpers.CleanupTmpDir(guid);
            }
        }
    }
}
