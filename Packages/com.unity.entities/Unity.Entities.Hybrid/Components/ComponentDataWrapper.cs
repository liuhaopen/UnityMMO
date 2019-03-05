using System;
using UnityEngine;

namespace Unity.Entities
{
    public abstract class ComponentDataWrapper<T> : ComponentDataWrapperBase where T : struct, IComponentData
    {
        internal override void ValidateSerializedData()
        {
            ValidateSerializedData(ref m_SerializedData);
        }

        protected virtual void ValidateSerializedData(ref T serializedData) {}

        [SerializeField, WrappedComponentData]
        T m_SerializedData;

        public T Value
        {
            get
            {
                return m_SerializedData;
            }
            set
            {
                ValidateSerializedData(ref value);
                m_SerializedData = value;

                EntityManager entityManager;
                Entity entity;

                if (CanSynchronizeWithEntityManager(out entityManager, out entity))
                    UpdateComponentData(entityManager, entity);
            }
        }

        internal override ComponentType GetComponentType()
        {
            return ComponentType.Create<T>();
        }

        internal override void UpdateComponentData(EntityManager manager, Entity entity)
        {
            if (!ComponentType.Create<T>().IsZeroSized)
                manager.SetComponentData(entity, m_SerializedData);
        }

        internal override void UpdateSerializedData(EntityManager manager, Entity entity)
        {
            if (!ComponentType.Create<T>().IsZeroSized)
                m_SerializedData = manager.GetComponentData<T>(entity);
        }

        internal override int InsertSharedComponent(EntityManager manager)
        {
            throw new InvalidOperationException();
        }

        internal override void UpdateSerializedData(EntityManager manager, int sharedComponentIndex)
        {
            throw new InvalidOperationException();
        }
    }
}