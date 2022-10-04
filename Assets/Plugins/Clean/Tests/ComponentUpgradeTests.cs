using System;
using System.IO;
using NUnit.Framework;
using Plugins.Clean.Editor;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Tests
{
    [TestFixture]
    public class ComponentUpgradeTests
    {
        [Test]
        public void CanUpgradeTextComponent()
        {
            var testText = "some text";
            var textComponent = Helpers.CreateText(testText);
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
            var guid = Helpers.SetupTmpDir();
            var prefabPath = $"Assets/{guid}/text.prefab";
            GameObject tmpInstance = null;

            try
            {
                // Create prefab
                Helpers.CreateTextPrefab("original text", prefabPath);

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
                Helpers.CleanupTmpDir(guid);
            }
        }

        [Test]
        public void ComponentUpgrade_Dropdown_WithChildren()
        {
            var prefab = PrefabUtility.LoadPrefabContents("Assets/Plugins/Clean/Tests/Prefabs/Dropdown.prefab");
            var go = Object.Instantiate(prefab);
            var dropdown = go.GetComponent<Dropdown>();

            Assert.AreEqual("Option A", dropdown.GetComponentsInChildren<Text>()[0].text);
            var tmpDropdown = ComponentUpgrade.UpgradeDropdown(dropdown);
            Assert.AreEqual(0, tmpDropdown.GetComponentsInChildren<Text>().Length);
            Assert.AreEqual("Option A", tmpDropdown.GetComponentsInChildren<TextMeshProUGUI>()[0].text);
        }
    }
}