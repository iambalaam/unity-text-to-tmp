using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Clean.Editor
{
    public static class ComponentProperties
    {
        public class SelectableObject
        {
            private readonly AnimationTriggers animationTriggers;
            private readonly ColorBlock colors;
            private readonly Image image;
            private readonly bool interactable;
            private readonly Navigation navigation;
            private readonly SpriteState spriteState;
            private readonly Graphic targetGraphic;
            private readonly Selectable.Transition transition;

            public SelectableObject( Selectable selectable )
            {
                animationTriggers = selectable.animationTriggers;
                colors = selectable.colors;
                image = selectable.image;
                interactable = selectable.interactable;
                navigation = selectable.navigation;
                spriteState = selectable.spriteState;
                targetGraphic = selectable.targetGraphic;
                transition = selectable.transition;
            }

            public Selectable Apply( Selectable selectable )
            {
                selectable.animationTriggers = animationTriggers;
                selectable.colors = colors;
                selectable.image = image;
                selectable.interactable = interactable;
                selectable.navigation = navigation;
                selectable.spriteState = spriteState;
                selectable.targetGraphic = targetGraphic;
                selectable.transition = transition;
                return selectable;
            }
        }
        
        public class Text
        {
            private Vector2 sizeDelta;
            private TextAnchor alignment;
            private bool alignByGeometry;
            private bool bestFit;
            private int bestFitMaxSize;
            private int bestFitMinSize;
            private Color color;
            private bool enabled;
            private Font font;
            private int fontSize;
            private FontStyle fontStyle;
            private HorizontalWrapMode horizontalOverflow;
            private float lineSpacing;
            private bool raycastTarget;
            private bool supportRichText;
            private string text;
            private VerticalWrapMode verticalOverflow;

            public Text(UnityEngine.UI.Text t)
            {
                sizeDelta = t.rectTransform.sizeDelta;
                alignment = t.alignment;
                alignByGeometry = t.alignByGeometry;
                bestFit = t.resizeTextForBestFit;
                bestFitMaxSize = t.resizeTextMaxSize;
                bestFitMinSize = t.resizeTextMinSize;
                color = t.color;
                enabled = t.enabled;
                font = t.font;
                fontSize = t.fontSize;
                fontStyle = t.fontStyle;
                horizontalOverflow = t.horizontalOverflow;
                verticalOverflow = t.verticalOverflow;
                lineSpacing = t.lineSpacing;
                raycastTarget = t.raycastTarget;
                supportRichText = t.supportRichText;
                text = t.text;
            }

            public TextMeshProUGUI Apply(TextMeshProUGUI tmp)
            {
                tmp.rectTransform.sizeDelta = sizeDelta;
                tmp.alignment = Util.GetTMPAlignment(alignment, alignByGeometry);
                tmp.enableAutoSizing = bestFit;
                tmp.fontSizeMin = bestFitMinSize;
                tmp.fontSizeMax = bestFitMaxSize;
                tmp.color = color;
                tmp.enabled = enabled;
                // TODO: Font + FontMaterial GetCorrespondingTMPFontAsset()
                tmp.fontSize = fontSize;
                tmp.fontStyle = Util.GetTMPFontStyle(fontStyle);
                tmp.enableWordWrapping = horizontalOverflow == HorizontalWrapMode.Wrap;
                tmp.lineSpacing = (lineSpacing - 1) * 100f;
                tmp.raycastTarget = raycastTarget;
                tmp.richText = supportRichText;
                tmp.text = text;
                // TODO: OverflowMode GetTMPVerticalOverflow()

                return tmp;
            }
        }

        public class TextMesh
        {
            private TextAnchor anchor;
            private float characterSize;
            private Color color;
            private Font font;
            private int fontSize;
            private FontStyle fontStyle;
            private float lineSpacing;
            private float offsetZ;
            private bool richText;
            private string text;
            

            public TextMesh(UnityEngine.TextMesh textMesh)
            {
                anchor = textMesh.anchor;
                characterSize = textMesh.characterSize;
                color = textMesh.color;
                font = textMesh.font;
                fontSize = textMesh.fontSize;
                fontStyle = textMesh.fontStyle;
                lineSpacing = textMesh.lineSpacing;
                offsetZ = textMesh.offsetZ;
                richText = textMesh.richText;
                text = textMesh.text;
            }
            
            public TextMeshPro Apply(TextMeshPro tmp)
            {
                tmp.alignment = Util.GetTMPAlignment(anchor, false);
                tmp.color = color;
                // TODO: textMeshPro.font = Util.GetCorrespondingTMPFontAsset();
                // TODO: textMeshPro.fontMaterial = Util.GetCorrespondingTMPFontAsset();?
                tmp.fontSize = fontSize > 0 ? fontSize : 13;
                tmp.fontStyle = Util.GetTMPFontStyle(fontStyle);
                tmp.lineSpacing = lineSpacing;
                tmp.richText = richText;
                tmp.text = text;
                tmp.enableWordWrapping = false;
                tmp.overflowMode = TextOverflowModes.Overflow;
                tmp.rectTransform.sizeDelta = Vector2.zero;
                tmp.rectTransform.localScale *= characterSize;
                tmp.rectTransform.Translate(Vector3.forward * offsetZ);

                return tmp;
            }
        }

        public class InputField
        {
            // Properties
            private bool enabled;
            private Vector2 sizeDelta;
            private SelectableObject selectable;
            private char asteriskChar;
            private float caretBlinkRate;
            private bool customCaretColor;
            private Color? caretColor;
            private float caretWidth;
            private int characterLimit;
            private UnityEngine.UI.InputField.CharacterValidation characterValidation;
            private UnityEngine.UI.InputField.ContentType contentType;
            private UnityEngine.UI.InputField.InputType inputType;
            private TouchScreenKeyboardType keyboardType;
            private UnityEngine.UI.InputField.LineType lineType;
            private bool readOnly;
            private Color selectionColor;
            private bool shouldHideMobileInput;
            private string text;
            // Unity events
            private object onEndEditEvent;
            private object onValueChangedEvent;

            public InputField(UnityEngine.UI.InputField input)
            {
                // Properties
                enabled = input.enabled;
                sizeDelta = ((RectTransform)input.transform).sizeDelta;
                selectable = new SelectableObject(input);
                asteriskChar = input.asteriskChar;
                caretBlinkRate = input.caretBlinkRate;
                customCaretColor = input.customCaretColor;
                if (customCaretColor) caretColor = input.caretColor; // This can throw
                caretWidth = input.caretWidth;
                characterLimit = input.characterLimit;
                characterValidation = input.characterValidation;
                contentType = input.contentType;
                inputType = input.inputType;
                keyboardType = input.keyboardType;
                lineType = input.lineType;
                readOnly = input.readOnly;
                selectionColor = input.selectionColor;
                shouldHideMobileInput = input.shouldHideMobileInput;
                text = input.text;
                // Unity events
                onEndEditEvent = Util.CopyUnityEvent(input.onEndEdit);
                onValueChangedEvent = Util.CopyUnityEvent(input.onValueChanged);
            }

            public TMP_InputField Apply(TMP_InputField tmpInput)
            {
                selectable.Apply(tmpInput);
                ((RectTransform)tmpInput.transform).sizeDelta = sizeDelta;
                tmpInput.enabled = enabled;
                tmpInput.asteriskChar = asteriskChar;
                tmpInput.caretBlinkRate = caretBlinkRate;
                tmpInput.customCaretColor = customCaretColor;
                tmpInput.keyboardType = keyboardType;
                tmpInput.selectionColor = selectionColor;
                tmpInput.shouldHideMobileInput = shouldHideMobileInput;
                tmpInput.characterLimit = characterLimit;
                tmpInput.readOnly = readOnly;
                tmpInput.text = text;
                if (caretColor.HasValue) tmpInput.caretColor = caretColor.Value;
                tmpInput.caretWidth = Mathf.RoundToInt(caretWidth);
                tmpInput.characterValidation = Util.GetTMPCharacterValidation(characterValidation);
                tmpInput.contentType = Util.GetTMPContentType(contentType);
                tmpInput.inputType = Util.GetTMPInputType(inputType);
                // Unity events
                Util.PasteUnityEvent(tmpInput.onEndEdit, onEndEditEvent);
                Util.PasteUnityEvent(tmpInput.onValueChanged, onValueChangedEvent);
                
                return tmpInput;
            }
        }

        public class Dropdown
        {
            private bool enabled;
            private Vector2 sizeDelta;
            private SelectableObject selectable;
            private Image captionImage;
            private Image itemImage;
            private RectTransform template;
            private int value;
            private List<UnityEngine.UI.Dropdown.OptionData> options;
            private object onValueChanged;
            
            public Dropdown(UnityEngine.UI.Dropdown dropdown)
            {
                enabled = dropdown.enabled;
                sizeDelta = ((RectTransform)dropdown.transform).sizeDelta;
                selectable = new SelectableObject(dropdown);
                captionImage = dropdown.captionImage;
                itemImage = dropdown.itemImage;
                template = dropdown.template;
                value = dropdown.value;
                options = dropdown.options;
                // Unity Events
                onValueChanged = Util.CopyUnityEvent(dropdown.onValueChanged);
            }
            public TMP_Dropdown Apply(TMP_Dropdown dropdown)
            {
                dropdown.enabled = enabled;
                ((RectTransform) dropdown.transform).sizeDelta = sizeDelta;
                selectable.Apply(dropdown);
                dropdown.captionImage = captionImage;
                dropdown.itemImage = itemImage;
                dropdown.options = Util.GetTMPDropdownOptions(options);
                dropdown.template = template;
                dropdown.value = value;
                // Unity Events
                Util.PasteUnityEvent(dropdown.onValueChanged, onValueChanged);
                
                return dropdown;
            }
        }
    }
}
