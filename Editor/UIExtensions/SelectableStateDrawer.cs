using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rehawk.UIFramework.UIExtensions
{
    [CustomPropertyDrawer(typeof(SelectableState), true)]
    public class SelectableStateDrawer : PropertyDrawer
    {
        private const string NORMAL_STATE_PROPERTY_NAME = "NormalState";
        private const string HIGHLIGHTED_STATE_PROPERTY_NAME = "HighlightedState";
        private const string PRESSED_STATE_PROPERTY_NAME = "PressedState";
        private const string SELECTED_STATE_PROPERTY_NAME = "SelectedState";
        private const string DISABLED_STATE_PROPERTY_NAME = "DisabledState";

        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = rect;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty normalStateProperty = prop.FindPropertyRelative(NORMAL_STATE_PROPERTY_NAME);
            SerializedProperty highlightedStateProperty = prop.FindPropertyRelative(HIGHLIGHTED_STATE_PROPERTY_NAME);
            SerializedProperty pressedStateProperty = prop.FindPropertyRelative(PRESSED_STATE_PROPERTY_NAME);
            SerializedProperty selectedStateProperty = prop.FindPropertyRelative(SELECTED_STATE_PROPERTY_NAME);
            SerializedProperty disabledStateProperty = prop.FindPropertyRelative(DISABLED_STATE_PROPERTY_NAME);

            EditorGUI.PropertyField(drawRect, normalStateProperty);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, highlightedStateProperty);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, pressedStateProperty);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, selectedStateProperty);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, disabledStateProperty);
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            SerializedProperty[] properties = new[]
            {
                property.FindPropertyRelative(NORMAL_STATE_PROPERTY_NAME),
                property.FindPropertyRelative(HIGHLIGHTED_STATE_PROPERTY_NAME),
                property.FindPropertyRelative(PRESSED_STATE_PROPERTY_NAME),
                property.FindPropertyRelative(SELECTED_STATE_PROPERTY_NAME),
                property.FindPropertyRelative(DISABLED_STATE_PROPERTY_NAME),
            };

            foreach (SerializedProperty prop in properties)
            {
                var field = new PropertyField(prop);
                container.Add(field);
            }

            return container;
        }
    }
}
