using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Editor
{
    public class Upgrade
    {
        private List<KeyValuePair<GameObject, ComponentProperties.Text>> textCache =
            new List<KeyValuePair<GameObject, ComponentProperties.Text>>();

        private List<KeyValuePair<GameObject, ComponentProperties.TextMesh>> textMeshCache =
            new List<KeyValuePair<GameObject, ComponentProperties.TextMesh>>();

        private List<KeyValuePair<GameObject, ComponentProperties.Dropdown>> dropdownCache =
            new List<KeyValuePair<GameObject, ComponentProperties.Dropdown>>();

        private List<KeyValuePair<GameObject, ComponentProperties.InputField>> inputFieldCache =
            new List<KeyValuePair<GameObject, ComponentProperties.InputField>>();

        private HashSet<string> updatedPrefabs = new HashSet<string>();

        private void CacheSceneOverrides(Scene scene)
        {
            foreach (var go in scene.GetRootGameObjects())
            {
                RecursivelyCacheOverrides(go);
            }
        }

        private void RecursivelyCacheOverrides(GameObject root)
        {
            Util.RecurseOverChildren(root, (go) =>
            {
                var text = go.GetComponent<Text>();
                var textMesh = go.GetComponent<TextMesh>();
                var inputField = go.GetComponent<InputField>();
                var dropdown = go.GetComponent<Dropdown>();
                if (text)
                    textCache.Add(
                        new KeyValuePair<GameObject, ComponentProperties.Text>(go,
                            new ComponentProperties.Text(text)));
                if (textMesh)
                    textMeshCache.Add(
                        new KeyValuePair<GameObject, ComponentProperties.TextMesh>(go,
                            new ComponentProperties.TextMesh(textMesh)));
                if (dropdown)
                    dropdownCache.Add(
                        new KeyValuePair<GameObject, ComponentProperties.Dropdown>(go,
                            new ComponentProperties.Dropdown(dropdown)));
                if (inputField)
                    inputFieldCache.Add(
                        new KeyValuePair<GameObject, ComponentProperties.InputField>(go,
                            new ComponentProperties.InputField(inputField)));
            });
        }

        private void UpgradeAllPrefabs(IEnumerable<string> prefabPaths)
        {
            foreach (var prefabPath in prefabPaths)
            {
                ComponentUpgrade.UpgradePrefabRoot(prefabPath);
            }
        }

        private void UpgradeAllSceneObjects(IEnumerable<Scene> scenes)
        {
            foreach (var scene in scenes)
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    ComponentUpgrade.RecursivelyUpgradeGameObject(root);
                }
            }
        }

        public void TextToTMP(Scene[] scenes, string[] prefabPaths)
        {
            foreach (var scene in scenes)
            {
                // Load and cache scenes
                if (scene.isDirty) throw new Exception($"Scene {scene.name}is not saved");
                if (!scene.isLoaded) SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
                CacheSceneOverrides(scene);

                UpgradeAllPrefabs(prefabPaths);
                UpgradeAllSceneObjects(scenes);

                // Reapply cached overrides
                foreach (var pair in textCache)
                    pair.Value.Apply(pair.Key.GetComponent<TextMeshProUGUI>());
                foreach (var pair in textMeshCache)
                    pair.Value.Apply(pair.Key.GetComponent<TextMeshPro>());
                foreach (var pair in inputFieldCache)
                    pair.Value.Apply(pair.Key.GetComponent<TMP_InputField>());
                foreach (var pair in dropdownCache)
                    pair.Value.Apply(pair.Key.GetComponent<TMP_Dropdown>());

            }
        }


    }
}
