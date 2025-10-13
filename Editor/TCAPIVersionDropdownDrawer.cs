using UnityEditor;
using UnityEngine;
using TinCan;
using System.Linq;

namespace OmiLAXR.xAPI.Editor
{
    // TCAPIVersionDropdownDrawer.cs
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TCAPIVersionDropdownAttribute))]
    public class TCAPIVersionDropdownDrawer : PropertyDrawer
    {
        private string[] _options;

        private string[] Options
        {
            get
            {
                if (_options == null)
                {
                    var dict = TCAPIVersion.GetSupported();
                    _options = dict.Keys
                        .OrderByDescending(s => new System.Version(s)) // 1.0.3 vor 1.0.2
                        .ToArray();
                }
                return _options;
            }
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            if (prop.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(pos, "[TCAPIVersionDropdown] requires a string field.", MessageType.Error);
                return;
            }

            EditorGUI.BeginProperty(pos, label, prop);
            var idx = System.Array.IndexOf(Options, prop.stringValue);
            if (idx < 0) idx = 0;

            var newIdx = EditorGUI.Popup(pos, label.text, idx, Options);
            if (newIdx != idx && newIdx >= 0 && newIdx < Options.Length)
                prop.stringValue = Options[newIdx];

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
            => EditorGUIUtility.singleLineHeight;
    }
#endif

}
