using System;
using TMPro;
using UnityEngine;

namespace Plugins.Clean.Editor
{
    public static class ComponentProperties
    {
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
            public TextMeshPro Apply()
            {
                throw new NotImplementedException();
            }
        }

        public class InputField
        {
            public TMP_InputField Apply()
            {
                throw new NotImplementedException();
            }
        }

        public class Dropdown
        {
            public TMP_Dropdown Apply()
            {
                throw new NotImplementedException();
            }
        }
    }
}
