using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Editor
{
    public static class Util
    {
        public static TextMeshProUGUI UpgradeText(Text t)
        {
            // Copy properties
            var go = t.gameObject;
            var properties = new ComponentProperties.Text(t);
            // Swap Text component for TextMeshPropUGUI
            Object.DestroyImmediate(t);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            // Paste new properties
            properties.Apply(tmp);
            return tmp;
        }

        public static TMP_InputField UpgradeInputField(InputField input)
        {
            // Copy properties
            var go = input.gameObject;
            // Copy Events
            // Swap InputField component with TMP_InputField
            var tmpInput = go.AddComponent<TMP_InputField>();
            // Paste new properties
            // Reapply events
            return tmpInput;
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

        // TODO: Investigate if there is/should be a public API for this
        private static FieldInfo GetUnityPersistentCalls()
        {
            var calls = typeof(UnityEventBase).GetField("m_PersistentCalls",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (calls != null) return calls;
            
            var listeners = typeof(UnityEventBase).GetField("m_PersistentListeners",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (listeners!= null) return listeners;

            throw new Exception("Could not retrieve m_PersistentCalls to copy Unity Events");
        }

        public static object CopyUnityEvent(UnityEventBase target)
        {
            var field = GetUnityPersistentCalls();
            return field.GetValue(target);
        }

        public static void PasteUnityEvent(UnityEventBase target, object unityEvent)
        {
            var field = GetUnityPersistentCalls();
            field.SetValue(target, unityEvent);
        }
    }
}