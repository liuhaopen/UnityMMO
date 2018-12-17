using System;
using Unity.Entities;

namespace Unity.Transforms
{
    /// <summary>
    /// Copy Transform from GameObject associated with Entity to TransformMatrix.
    /// </summary>
    [Serializable]
    public struct CopyTransformFromGameObject : IComponentData { }

    [UnityEngine.DisallowMultipleComponent]
    public class CopyTransformFromGameObjectComponent : ComponentDataWrapper<CopyTransformFromGameObject> { } 
}
