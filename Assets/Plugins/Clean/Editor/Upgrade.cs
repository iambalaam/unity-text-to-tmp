using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Plugins.Clean.Editor
{
    public class Upgrade
    {
        private Dictionary<GameObject, ComponentProperties.Text> textCache =
            new Dictionary<GameObject, ComponentProperties.Text>();
        private Dictionary<GameObject, ComponentProperties.TextMesh> textMeshCache =
            new Dictionary<GameObject, ComponentProperties.TextMesh>();
        private Dictionary<GameObject, ComponentProperties.Dropdown> dropdownCache =
            new Dictionary<GameObject, ComponentProperties.Dropdown>();
        private Dictionary<GameObject, ComponentProperties.InputField> inputFieldCache =
            new Dictionary<GameObject, ComponentProperties.InputField>();

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
                if (text) textCache.Add(go, new ComponentProperties.Text(text));
                if (textMesh) textMeshCache.Add(go, new ComponentProperties.TextMesh(textMesh));
                if (dropdown) dropdownCache.Add(go, new ComponentProperties.Dropdown(dropdown));
                if (inputField) inputFieldCache.Add(go, new ComponentProperties.InputField(inputField));

            });
        }
    }
}
