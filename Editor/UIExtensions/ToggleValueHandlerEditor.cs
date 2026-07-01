using Rehawk.UIFramework.UIExtensions;
using UnityEditor;
using UnityEngine;

namespace Rehawk.UIFramework.Editor.UIExtensions
{
    [CustomEditor(typeof(ToggleValueHandlerBase), true)]
    public class ToggleValueHandlerEditor : UnityEditor.Editor
    {
        private const string SCRIPT_PROPERTY_NAME = "m_Script";

        private PreviewValue _previewValue = PreviewValue.ToggleValue;
        private bool _hasLastToggleValueHash;
        private int _lastToggleValueHash;

        private void OnDisable()
        {
            Object[] restoreTargets = targets;
            EditorApplication.delayCall += () => RestoreToggleValueIfDeselected(restoreTargets);
            _hasLastToggleValueHash = false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();
            bool enterChildren = true;

            while (property.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (property.name == SCRIPT_PROPERTY_NAME)
                    continue;

                EditorGUILayout.PropertyField(property, true);
            }

            bool serializedPropertiesChanged = serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            _previewValue = (PreviewValue)EditorGUILayout.EnumPopup("Preview Value", _previewValue);
            bool previewValueChanged = EditorGUI.EndChangeCheck();
            bool toggleValueChanged = _previewValue == PreviewValue.ToggleValue && HasToggleValueChanged();

            if (previewValueChanged || serializedPropertiesChanged || toggleValueChanged)
            {
                ApplyPreviewValue();
            }
        }

        private bool HasToggleValueChanged()
        {
            int toggleValueHash = GetToggleValueHash();

            if (!_hasLastToggleValueHash)
            {
                _lastToggleValueHash = toggleValueHash;
                _hasLastToggleValueHash = true;
                return false;
            }

            if (_lastToggleValueHash == toggleValueHash)
                return false;

            _lastToggleValueHash = toggleValueHash;
            return true;
        }

        private int GetToggleValueHash()
        {
            int hash = 17;

            foreach (Object targetObject in targets)
            {
                if (targetObject is ToggleValueHandlerBase toggleValueHandler)
                {
                    hash = hash * 31 + (toggleValueHandler.EditorCurrentToggleValue ? 1 : 0);
                }
            }

            return hash;
        }

        private void ApplyPreviewValue()
        {
            foreach (Object targetObject in targets)
            {
                if (targetObject is not ToggleValueHandlerBase toggleValueHandler)
                    continue;

                switch (_previewValue)
                {
                    case PreviewValue.Off:
                        toggleValueHandler.PreviewValueTransition(false);
                        break;
                    case PreviewValue.On:
                        toggleValueHandler.PreviewValueTransition(true);
                        break;
                    default:
                        toggleValueHandler.PreviewToggleValue();
                        break;
                }
            }
        }

        private static void RestoreToggleValueIfDeselected(Object[] restoreTargets)
        {
            foreach (Object targetObject in restoreTargets)
            {
                if (targetObject is ToggleValueHandlerBase toggleValueHandler)
                {
                    if (IsStillSelected(toggleValueHandler))
                        continue;

                    toggleValueHandler.RestoreToggleValue();
                }
            }
        }

        private static bool IsStillSelected(Component component)
        {
            if (!component)
                return false;

            foreach (Object selectedObject in Selection.objects)
            {
                if (selectedObject == component || selectedObject == component.gameObject)
                {
                    return true;
                }
            }

            return false;
        }

        private enum PreviewValue
        {
            ToggleValue,
            Off,
            On,
        }
    }
}
