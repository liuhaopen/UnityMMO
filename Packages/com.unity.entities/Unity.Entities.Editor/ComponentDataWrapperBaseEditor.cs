using System;
using System.Linq;
using System.Reflection;
using Unity.Entities.Serialization;
using UnityEditor;
using UnityEngine;

namespace Unity.Entities.Editor
{
    [CustomEditor(typeof(ComponentDataWrapperBase), true), CanEditMultipleObjects]
    public class ComponentDataWrapperBaseEditor : UnityEditor.Editor
    {
        string m_SerializableError;
        string m_MultipleComponentsWarning;
        string m_DisallowMultipleWarning;

        protected virtual void OnEnable()
        {
            var type = target.GetType();
            var multipleInstances = targets
                .Select(t => (t as ComponentDataWrapperBase).GetComponents(type))
                .Where(c => c.Length > 1)
                .ToArray();
            if (multipleInstances.Length > 0)
                m_MultipleComponentsWarning = string.Format(
                    L10n.Tr("{0} has multiple instances of {1}, but Entity may only have a single instance of any component type."),
                    multipleInstances[0][0].gameObject.name, type
                );
            var disallowMultipleType = Attribute.IsDefined(type, typeof(DisallowMultipleComponent), true) ? type : null;

            FieldInfo field = null;
            while (type.BaseType != typeof(ComponentDataWrapperBase))
            {
                type = type.BaseType;
                if (disallowMultipleType == null && Attribute.IsDefined(type, typeof(DisallowMultipleComponent), true))
                    disallowMultipleType = type;
                if (field == null)
                    field = type.GetField("m_SerializedData", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (field == null)
                return;

            if (disallowMultipleType != null && field.FieldType.GetInterfaces().Contains(typeof(ISharedComponentData)))
                m_DisallowMultipleWarning = string.Format(
                    L10n.Tr("Wrapper type {0} is marked with {1}, which is not currently compatible with {2}."),
                    disallowMultipleType, typeof(DisallowMultipleComponent), typeof(SerializeUtilityHybrid)
                );

            var serializedDataProperty = serializedObject.FindProperty("m_SerializedData");
            if (
                serializedDataProperty == null
                && !Attribute.IsDefined(field, typeof(SerializableAttribute))
                && field.FieldType.GetFields(BindingFlags.Public | BindingFlags.Instance).Length > 0
            )
            {
                m_SerializableError = string.Format(
                    L10n.Tr("Component type {0} is not marked with {1}."), field.FieldType, typeof(SerializableAttribute)
                );
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!string.IsNullOrEmpty(m_SerializableError))
                EditorGUILayout.HelpBox(m_SerializableError, MessageType.Error);
            if (!string.IsNullOrEmpty(m_MultipleComponentsWarning))
                EditorGUILayout.HelpBox(m_MultipleComponentsWarning, MessageType.Warning);
            if (!string.IsNullOrEmpty(m_DisallowMultipleWarning))
                EditorGUILayout.HelpBox(m_DisallowMultipleWarning, MessageType.Warning);
        }
    }
}
