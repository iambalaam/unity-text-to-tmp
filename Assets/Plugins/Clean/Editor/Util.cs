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
        
        public static TMP_InputField.CharacterValidation GetTMPCharacterValidation( InputField.CharacterValidation characterValidation )
        {
            switch( characterValidation )
            {
                case InputField.CharacterValidation.Alphanumeric: return TMP_InputField.CharacterValidation.Alphanumeric;
                case InputField.CharacterValidation.Decimal: return TMP_InputField.CharacterValidation.Decimal;
                case InputField.CharacterValidation.EmailAddress: return TMP_InputField.CharacterValidation.EmailAddress;
                case InputField.CharacterValidation.Integer: return TMP_InputField.CharacterValidation.Integer;
                case InputField.CharacterValidation.Name: return TMP_InputField.CharacterValidation.Name;
                case InputField.CharacterValidation.None: return TMP_InputField.CharacterValidation.None;
                default: return TMP_InputField.CharacterValidation.None;
            }
        }
        
        public static TMP_InputField.ContentType GetTMPContentType( InputField.ContentType contentType )
        {
            switch( contentType )
            {
                case InputField.ContentType.Alphanumeric: return TMP_InputField.ContentType.Alphanumeric;
                case InputField.ContentType.Autocorrected: return TMP_InputField.ContentType.Autocorrected;
                case InputField.ContentType.Custom: return TMP_InputField.ContentType.Custom;
                case InputField.ContentType.DecimalNumber: return TMP_InputField.ContentType.DecimalNumber;
                case InputField.ContentType.EmailAddress: return TMP_InputField.ContentType.EmailAddress;
                case InputField.ContentType.IntegerNumber: return TMP_InputField.ContentType.IntegerNumber;
                case InputField.ContentType.Name: return TMP_InputField.ContentType.Name;
                case InputField.ContentType.Password: return TMP_InputField.ContentType.Password;
                case InputField.ContentType.Pin: return TMP_InputField.ContentType.Pin;
                case InputField.ContentType.Standard: return TMP_InputField.ContentType.Standard;
                default: return TMP_InputField.ContentType.Standard;
            }
        }
        
        public static TMP_InputField.InputType GetTMPInputType( InputField.InputType inputType )
        {
            switch( inputType )
            {
                case InputField.InputType.AutoCorrect: return TMP_InputField.InputType.AutoCorrect;
                case InputField.InputType.Password: return TMP_InputField.InputType.Password;
                case InputField.InputType.Standard: return TMP_InputField.InputType.Standard;
                default: return TMP_InputField.InputType.Standard;
            }
        }

        public static TMP_InputField.LineType GetTMPLineType( InputField.LineType lineType )
        {
            switch( lineType )
            {
                case InputField.LineType.MultiLineNewline: return TMP_InputField.LineType.MultiLineNewline;
                case InputField.LineType.MultiLineSubmit: return TMP_InputField.LineType.MultiLineSubmit;
                case InputField.LineType.SingleLine: return TMP_InputField.LineType.SingleLine;
                default: return TMP_InputField.LineType.SingleLine;
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