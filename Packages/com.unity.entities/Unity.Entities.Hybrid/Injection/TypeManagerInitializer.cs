using UnityEngine;

namespace Unity.Entities
{
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad] // ensures type manager is initialized on domain reload when not playing
    #endif
    static class TypeManagerInitializer
    {
        static TypeManagerInitializer()
        {
            InitializeTypeManager();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeTypeManager()
        {
            TypeManager.UnityEngineComponentType = typeof(Component);
        }
    }
}
