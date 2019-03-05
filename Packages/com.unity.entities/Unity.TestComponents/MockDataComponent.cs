using System;
using UnityEngine;

namespace Unity.Entities.Tests
{
    public interface IIntegerContainer
    {
        int Integer { get; set; }
    }

    [Serializable]
    public struct MockData : IComponentData
    {
        public int Value;
    }

    [DisallowMultipleComponent]
    public class MockDataComponent : ComponentDataWrapper<MockData>, IIntegerContainer
    {
        public int Integer
        {
            get { return Value.Value; }
            set { Value = new MockData {Value = value}; }
        }
    }
}