using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Editor
{
    public static class ComponentUpgrade
    {
        public static TextMeshProUGUI UpgradeText(Text t)
        {
            // Copy properties
            var go = t.gameObject;
            var properties = new ComponentProperties.Text(t);

            // Swap Text component for TextMeshPropUGUI
            Object.DestroyImmediate(t, true);
            var tmp = go.AddComponent<TextMeshProUGUI>();

            // Paste new properties
            properties.Apply(tmp);
            return tmp;
        }

        public static TMP_InputField UpgradeInputField(InputField input)
        {
            // Copy properties
            var go = input.gameObject;
            var properties = new ComponentProperties.InputField(input);

            // Upgrade child components
            var textComponent = UpgradeText(input.textComponent);
            Graphic placeholderComponent = (input.placeholder as Text)
                ? UpgradeText((Text)input.placeholder)
                : input.placeholder;

            // Swap InputField component with TMP_InputField
            Object.DestroyImmediate(input, true);
            var tmpInput = go.AddComponent<TMP_InputField>();

            // Paste child components
            tmpInput.textComponent = textComponent;
            tmpInput.placeholder = placeholderComponent;

            // Paste new properties
            properties.Apply(tmpInput);

            // TMP InputField objects have an extra Viewport (Text Area) child object, create it if necessary
            if (textComponent)
            {
                RectTransform viewport;
                if (textComponent.transform.parent != tmpInput.transform)
                    viewport = (RectTransform)textComponent.transform.parent;
                else
                    viewport = Util.CreateInputFieldViewport(tmpInput, textComponent, placeholderComponent);

                if (!viewport.GetComponent<RectMask2D>())
                    viewport.gameObject.AddComponent<RectMask2D>();

                tmpInput.textViewport = viewport;
            }

            return tmpInput;
        }

        public static TextMeshPro UpgradeTextMesh(TextMesh textMesh)
        {
            // Copy properties
            var go = textMesh.gameObject;
            var properties = new ComponentProperties.TextMesh(textMesh);

            // Replace TextMesh with TextMeshPro
            Object.DestroyImmediate(textMesh);
            var tmp = go.AddComponent<TextMeshPro>();

            // Paste properties
            properties.Apply(tmp);

            return tmp;
        }

        public static TMP_Dropdown UpgradeDropdown(Dropdown dropdown)
        {
            // Copy properties
            var go = dropdown.gameObject;
            var properties = new ComponentProperties.Dropdown(dropdown);

            // Upgrade child components
            TextMeshProUGUI captionText = null;
            if (dropdown.captionText) captionText = UpgradeText(dropdown.captionText);
            TextMeshProUGUI itemText = null;
            if (dropdown.itemText) itemText = UpgradeText(dropdown.itemText);

            // Replace Dropdown with TMP_Dropdown
            Object.DestroyImmediate(dropdown, true);
            var tmpDropdown = go.AddComponent<TMP_Dropdown>();

            // Paste child components
            if (captionText) tmpDropdown.captionText = captionText;
            if (itemText) tmpDropdown.itemText = itemText;

            // Paste properties
            properties.Apply(tmpDropdown);

            return tmpDropdown;
        }

        public static void RecursivelyUpgradeGameObject(GameObject root)
        {
            Util.RecurseOverChildren(root, (go) =>
            {
                // ComponentUpgrade prefab
                if (PrefabUtility.IsAnyPrefabInstanceRoot(go))
                {
                    UpgradePrefabRoot(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go));
                }

                // ComponentUpgrade components
                var text = go.GetComponent<Text>();
                var textMesh = go.GetComponent<TextMesh>();
                var inputField = go.GetComponent<InputField>();
                var dropdown = go.GetComponent<Dropdown>();
                if (text) UpgradeText(text);
                if (textMesh) UpgradeTextMesh(textMesh);
                if (inputField) UpgradeInputField(inputField);
                if (dropdown) UpgradeDropdown(dropdown);
            });
        }

        public static void UpgradePrefabRoot(string prefabPath)
        {
            GameObject prefabInstanceRoot = null;
            try
            {
                // Check we can upgrade
                var prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);
                if (!Util.CanPrefabBeUpgraded(prefab)) return;

                // Load contents + upgrade
                prefabInstanceRoot = PrefabUtility.LoadPrefabContents(prefabPath);
                RecursivelyUpgradeGameObject(prefabInstanceRoot);

                prefab = PrefabUtility.SaveAsPrefabAsset(prefabInstanceRoot, prefabPath);
                EditorUtility.SetDirty(prefab);
            }
            finally
            {
                if (prefabInstanceRoot)
                {
                    PrefabUtility.UnloadPrefabContents(prefabInstanceRoot);
                }
            }
        }
    }
}