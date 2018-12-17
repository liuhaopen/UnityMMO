using System;
using Unity.Entities;

namespace Unity.Transforms
{
    /// <summary>
    /// Side-Channel input for attaching child to parent transforms.
    /// To use: Create new entity with Attach component.
    /// On TransformSystem update, Attached and Parent components will
    /// be added to child.
    /// To detach: Remove Attached component from child.
    /// To change parent: Create new entity with Attach component defining new relationship.
    /// </summary>
    [Serializable]
    public struct Attach : IComponentData
    {
        public Entity Parent;
        public Entity Child;
    }

    [UnityEngine.DisallowMultipleComponent]
    public class AttachComponent : ComponentDataWrapper<Attach>
    {
    }
}
