using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Plugins.Clean.Editor
{
    public static class Util
    {
        public static RectTransform CreateInputFieldViewport(TMP_InputField tmp, TextMeshProUGUI textComponent,
            Graphic placeholderComponent)
        {
            RectTransform viewport = null;
            try
            {
                viewport = (RectTransform)new GameObject("Text Area", typeof(RectTransform)).transform;
                viewport.transform.SetParent(tmp.transform, false);
                viewport.SetSiblingIndex(textComponent.rectTransform.GetSiblingIndex());
                viewport.localPosition = textComponent.rectTransform.localPosition;
                viewport.localRotation = textComponent.rectTransform.localRotation;
                viewport.localScale = textComponent.rectTransform.localScale;
                viewport.anchorMin = textComponent.rectTransform.anchorMin;
                viewport.anchorMax = textComponent.rectTransform.anchorMax;
                viewport.pivot = textComponent.rectTransform.pivot;
                viewport.anchoredPosition = textComponent.rectTransform.anchoredPosition;
                viewport.sizeDelta = textComponent.rectTransform.sizeDelta;

#if UNITY_2018_3_OR_NEWER
                PrefabUtility.RecordPrefabInstancePropertyModifications(viewport.gameObject);
                PrefabUtility.RecordPrefabInstancePropertyModifications(viewport.transform);
#endif

                for (int i = tmp.transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = tmp.transform.GetChild(i);
                    if (child == viewport)
                        continue;

                    if (child == textComponent.rectTransform ||
                        (placeholderComponent && child == placeholderComponent.rectTransform))
                    {
                        child.SetParent(viewport, true);
                        child.SetSiblingIndex(0);

#if UNITY_2018_3_OR_NEWER
                        PrefabUtility.RecordPrefabInstancePropertyModifications(child);
#endif
                    }
                }
            }
            catch
            {
                if (viewport)
                {
                    Object.DestroyImmediate(viewport);
                }

                throw;
            }

            return viewport;
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

        public static List<TMP_Dropdown.OptionData> GetTMPDropdownOptions(List<Dropdown.OptionData> options)
        {
            if (options == null) return null;
            var newOptions = new List<TMP_Dropdown.OptionData>(options.Count);
            foreach (var option in options)
            {
                newOptions.Add(new TMP_Dropdown.OptionData(option.text, option.image));
            }
            
            return newOptions;
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

        public static bool CanPrefabBeUpgraded(Object prefab)
        {
            if (!prefab) return false;
            if ((prefab.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable) return false;
            var type = PrefabUtility.GetPrefabAssetType(prefab);
            if (type != PrefabAssetType.Regular && type != PrefabAssetType.Variant) return false;

            return true;
        }

        public static void RecurseOverChildren(GameObject root, Action<GameObject> action)
        {
            action(root);
            for (int i = 0; i < root.transform.childCount; i++)
            {
                RecurseOverChildren(root.transform.GetChild(i).gameObject, action);
            }
        }
    }
}