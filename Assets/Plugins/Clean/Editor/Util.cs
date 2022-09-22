using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Editor
{
    public static class Util
    {
        public static TextMeshProUGUI UpgradeText(Text t)
        {
            var go = t.gameObject;
            var properties = new ComponentProperties.Text(t);
            Object.DestroyImmediate(t);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            properties.Apply(tmp);
            return tmp;
        }
        
        public static TextAlignmentOptions GetTMPAlignment( TextAnchor alignment, bool alignByGeometry )
        {
            switch( alignment )
            {
                case TextAnchor.LowerLeft: return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerRight: return TextAlignmentOptions.BottomRight;
                case TextAnchor.UpperLeft: return TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperRight: return TextAlignmentOptions.TopRight;
                case TextAnchor.LowerCenter: return alignByGeometry ? TextAlignmentOptions.BottomGeoAligned : TextAlignmentOptions.Bottom;
                case TextAnchor.MiddleLeft: return alignByGeometry ? TextAlignmentOptions.MidlineLeft : TextAlignmentOptions.Left;
                case TextAnchor.MiddleCenter: return alignByGeometry ? TextAlignmentOptions.MidlineGeoAligned : TextAlignmentOptions.Center;
                case TextAnchor.MiddleRight: return alignByGeometry ? TextAlignmentOptions.MidlineRight : TextAlignmentOptions.Right;
                case TextAnchor.UpperCenter: return alignByGeometry ? TextAlignmentOptions.TopGeoAligned : TextAlignmentOptions.Top;
                default: return alignByGeometry ? TextAlignmentOptions.MidlineGeoAligned : TextAlignmentOptions.Center;
            }
        }

        public static TextOverflowModes GetTMPVerticalOverflow()
        {
            throw new NotImplementedException();
        }
        
        public static FontStyles GetTMPFontStyle( FontStyle fontStyle )
        {
            switch( fontStyle )
            {
                case FontStyle.Bold: return FontStyles.Bold;
                case FontStyle.Italic: return FontStyles.Italic;
                case FontStyle.BoldAndItalic: return FontStyles.Bold | FontStyles.Italic;
                default: return FontStyles.Normal;
            }
        }
    }
}