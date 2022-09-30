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

            // ComponentUpgrade child components
            TextMeshProUGUI textComponent = UpgradeText(input.textComponent);
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
            throw new NotImplementedException();
        }
        public static TMP_Dropdown UpgradeDropdown(Dropdown dropdown)
        {
            throw new NotImplementedException();
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
                if (!CanPrefabBeUpgraded(prefab)) return;

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

        public static bool CanPrefabBeUpgraded(Object prefab)
        {
            if (!prefab) return false;
            if ((prefab.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable) return false;
            var type = PrefabUtility.GetPrefabAssetType(prefab);
            if (type != PrefabAssetType.Regular && type != PrefabAssetType.Variant) return false;

            return true;
        }
    }
}