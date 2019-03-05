using System;

namespace Unity.Entities
{
    public interface IComponentData
    {
    }

    public interface IBufferElementData
    {
    }

    public class InternalBufferCapacityAttribute : Attribute
    {
        public readonly int Capacity;

        public InternalBufferCapacityAttribute(int capacity)
        {
            Capacity = capacity;
        }
    }

    public interface ISharedComponentData
    {
    }

    public interface ISystemStateComponentData : IComponentData
    {
    }

    public interface ISystemStateSharedComponentData : ISharedComponentData
    {
    }

    public struct Disabled : IComponentData
    {
    }
    
    public struct Prefab : IComponentData
    {
    }
    
    public struct LinkedEntityGroup : IBufferElementData
    {
        public Entity Value;
        
        public static implicit operator LinkedEntityGroup(Entity e)
        {
            return new LinkedEntityGroup {Value = e};
        }

    }
}
