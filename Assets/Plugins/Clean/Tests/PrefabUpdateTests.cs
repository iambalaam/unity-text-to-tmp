using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Clean.Tests
{
    public class PrefabUpdateTests
    {
        private string assetPath = "Assets/tmp";

        [OneTimeSetUp]
        public void CreateTmpDir()
        {
            if (!Directory.Exists(assetPath))
                AssetDatabase.CreateFolder("Assets", "tmp");
        }

        [Test]
        public void CanCreateAPrefab()
        {
            var go = new GameObject();
            var text = go.AddComponent<Text>();
            text.text = "original text from prefab";

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, $"{assetPath}/text.prefab", out var hasSaved);
            Assert.IsTrue(hasSaved, "Could not create prefab");

            var newInstance = Object.Instantiate(prefab);
            var newInstanceText = newInstance.GetComponent<Text>();
            newInstanceText.text = "new updated text";

            Assert.AreEqual(text.text, "original text from prefab");
            Assert.AreEqual(newInstanceText.text, "new updated text"); ;
        }

        [OneTimeTearDown]
        public void RemoveTempDir()
        {
            if (Directory.Exists(assetPath))
                FileUtil.DeleteFileOrDirectory(assetPath);
        }
    }
}