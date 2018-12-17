using System;
using Unity.Entities;

namespace Unity.Rendering
{
    [Serializable]
    public struct MeshInstanceFlippedWindingTag : IComponentData
    {
    }

    [UnityEngine.DisallowMultipleComponent]
    public class MeshInstanceFlippedWindingTagComponent : ComponentDataWrapper<MeshInstanceFlippedWindingTag> { }
}
