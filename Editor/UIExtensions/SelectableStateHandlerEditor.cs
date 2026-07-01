using Rehawk.UIFramework.UIExtensions;
using UnityEditor;
using UnityEngine;

namespace Rehawk.UIFramework.Editor.UIExtensions
{
    [CustomEditor(typeof(SelectableStateHandlerBase), true)]
    public class SelectableStateHandlerEditor : UnityEditor.Editor
    {
        private const string SCRIPT_PROPERTY_NAME = "m_Script";

        private SelectableStateHandlerBase.SelectionState _previewState =
            SelectableStateHandlerBase.SelectionState.Normal;

        private void OnDisable()
        {
            Object[] restoreTargets = targets;
            EditorApplication.delayCall += () => RestoreCurrentStateIfDeselected(restoreTargets);
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
            _previewState = (SelectableStateHandlerBase.SelectionState)EditorGUILayout.EnumPopup("Preview State", _previewState);

            if (EditorGUI.EndChangeCheck() || serializedPropertiesChanged)
            {
                ApplyPreviewState();
            }
        }

        private void ApplyPreviewState()
        {
            foreach (Object targetObject in targets)
            {
                if (targetObject is SelectableStateHandlerBase selectableStateHandler)
                {
                    selectableStateHandler.PreviewStateTransition(_previewState);
                }
            }
        }

        private static void RestoreCurrentStateIfDeselected(Object[] restoreTargets)
        {
            foreach (Object targetObject in restoreTargets)
            {
                if (targetObject is SelectableStateHandlerBase selectableStateHandler)
                {
                    if (IsStillSelected(selectableStateHandler))
                        continue;

                    selectableStateHandler.RestoreCurrentStateTransition();
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
    }
}
