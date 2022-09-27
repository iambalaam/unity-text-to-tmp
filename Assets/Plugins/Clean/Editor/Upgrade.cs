using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Clean.Editor
{
    public static class Upgrade
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
            if( textComponent )
            {
                RectTransform viewport;
                if( textComponent.transform.parent != tmpInput.transform )
                    viewport = (RectTransform) textComponent.transform.parent;
                else
                    viewport = Util.CreateInputFieldViewport( tmpInput, textComponent, placeholderComponent );

                if( !viewport.GetComponent<RectMask2D>() )
                    viewport.gameObject.AddComponent<RectMask2D>();

                tmpInput.textViewport = viewport;
            }
            
            return tmpInput;
        }
    }
}