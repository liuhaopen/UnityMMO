using System;

namespace Unity.Entities.Tests
{
    [Serializable]
    public struct MockSharedData : ISharedComponentData
    {
        public int Value;
    }

    public class MockSharedDataComponent : SharedComponentDataWrapper<MockSharedData>, IIntegerContainer
    {
        public int Integer
        {
            get { return Value.Value; }
            set { Value = new MockSharedData {Value = value}; }
        }
    }
}