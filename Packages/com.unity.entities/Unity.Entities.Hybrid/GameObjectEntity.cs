using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RequireComponent = UnityEngine.RequireComponent;
using SerializeField = UnityEngine.SerializeField;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using GameObject = UnityEngine.GameObject;
using Component = UnityEngine.Component;

namespace Unity.Entities
{
    //@TODO: This should be fully implemented in C++ for efficiency
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [RequireComponent(typeof(GameObjectEntity))]
    public abstract class ComponentDataWrapperBase : MonoBehaviour, ISerializationCallbackReceiver
    {
        internal abstract ComponentType GetComponentType();
        internal abstract void UpdateComponentData(EntityManager manager, Entity entity);
        internal abstract void UpdateSerializedData(EntityManager manager, Entity entity);

        internal abstract int InsertSharedComponent(EntityManager manager);
        internal abstract void UpdateSerializedData(EntityManager manager, int sharedComponentIndex);

        internal abstract void ValidateSerializedData();

        protected virtual void OnEnable()
        {
            EntityManager entityManager;
            Entity entity;
            if (
                World.Active != null
                && TryGetEntityAndManager(out entityManager, out entity)
                && !entityManager.HasComponent(entity, GetComponentType()) // in case GameObjectEntity already added
            )
                entityManager.AddComponent(entity, GetComponentType());
        }

        protected virtual void OnDisable()
        {
            if (!gameObject.activeInHierarchy) // GameObjectEntity will handle removal when Entity is destroyed
                return;
            EntityManager entityManager;
            Entity entity;
            if (CanSynchronizeWithEntityManager(out entityManager, out entity))
                entityManager.RemoveComponent(entity, GetComponentType());
        }

        internal bool TryGetEntityAndManager(out EntityManager entityManager, out Entity entity)
        {
            entityManager = null;
            entity = Entity.Null;
            var gameObjectEntity = GetComponent<GameObjectEntity>();
            if (gameObjectEntity == null)
                return false;
            if (gameObjectEntity.EntityManager == null)
                return false;
            if (!gameObjectEntity.EntityManager.Exists(gameObjectEntity.Entity))
                return false;
            entityManager = gameObjectEntity.EntityManager;
            entity = gameObjectEntity.Entity;
            return true;
        }

        internal bool CanSynchronizeWithEntityManager(out EntityManager entityManager, out Entity entity)
        {
            return TryGetEntityAndManager(out entityManager, out entity)
                   && entityManager.HasComponent(entity, GetComponentType());
        }
        
        void OnValidate()
        {
            ValidateSerializedData();
            EntityManager entityManager;
            Entity entity;
            if (CanSynchronizeWithEntityManager(out entityManager, out entity))
                UpdateComponentData(entityManager, entity);
        }
        
        public void OnBeforeSerialize()
        {
            EntityManager entityManager;
            Entity entity;
            if (CanSynchronizeWithEntityManager(out entityManager, out entity))
                UpdateSerializedData(entityManager, entity);
        }

        public void OnAfterDeserialize() { }
    }

    internal sealed class WrappedComponentDataAttribute : PropertyAttribute
    {
    }

    //@TODO: This should be fully implemented in C++ for efficiency
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

    //@TODO: This should be fully implemented in C++ for efficiency
    public abstract class SharedComponentDataWrapper<T> : ComponentDataWrapperBase where T : struct, ISharedComponentData
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
            manager.SetSharedComponentData(entity, m_SerializedData);
        }
        
        internal override void UpdateSerializedData(EntityManager manager, Entity entity)
        {
            m_SerializedData = manager.GetSharedComponentData<T>(entity);
        }

        internal override int InsertSharedComponent(EntityManager manager)
        {
            return manager.m_SharedComponentManager.InsertSharedComponent(m_SerializedData);
        }

        internal override void UpdateSerializedData(EntityManager manager, int sharedComponentIndex)
        {
            m_SerializedData = manager.m_SharedComponentManager.GetSharedComponentData<T>(sharedComponentIndex);
        }
    }

    [DisallowMultipleComponent]
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    public class GameObjectEntity : MonoBehaviour
    {
        public EntityManager EntityManager { get; private set; }

        public Entity Entity { get; private set; }

        //@TODO: Very wrong error messages when creating entity with empty ComponentType array?

        public static Entity AddToEntityManager(EntityManager entityManager, GameObject gameObject)
        {
            ComponentType[] types;
            Component[] components;
            GetComponents(gameObject, true, out types, out components);

            var archetype = entityManager.CreateArchetype(types);
            var entity = CreateEntity(entityManager, archetype, components, types);

            return entity;
        }

        static void GetComponents(GameObject gameObject, bool includeGameObjectComponents, out ComponentType[] types, out Component[] components)
        {
            components = gameObject.GetComponents<Component>();

            var componentCount = 0;
            if (includeGameObjectComponents)
            {
                var gameObjectEntityComponent = gameObject.GetComponent<GameObjectEntity>();
                componentCount = gameObjectEntityComponent == null ? components.Length : components.Length - 1;
            }
            else
            {
                for (var i = 0; i != components.Length; i++)
                {
                    if (components[i] is ComponentDataWrapperBase)
                        componentCount++;
                }
            }

            types = new ComponentType[componentCount];

            var t = 0;
            for (var i = 0; i != components.Length; i++)
            {
                var com = components[i];
                var componentData = com as ComponentDataWrapperBase;

                if (componentData != null)
                    types[t++] = componentData.GetComponentType();
                else if (includeGameObjectComponents && !(com is GameObjectEntity))
                    types[t++] = com.GetType();
            }
        }

        static Entity CreateEntity(EntityManager entityManager, EntityArchetype archetype, IReadOnlyList<Component> components, IReadOnlyList<ComponentType> types)
        {
            var entity = entityManager.CreateEntity(archetype);
            var t = 0;
            for (var i = 0; i != components.Count; i++)
            {
                var com = components[i];
                var componentDataWrapper = com as ComponentDataWrapperBase;

                if (componentDataWrapper != null)
                {
                    componentDataWrapper.UpdateComponentData(entityManager, entity);
                    t++;
                }
                else if (!(com is GameObjectEntity))
                {
                    entityManager.SetComponentObject(entity, types[t], com);
                    t++;
                }
            }
            return entity;
        }

        protected virtual void OnEnable()
        {
            #if UNITY_EDITOR
            if (World.Active == null)
            {
                // * OnDisable (Serialize monobehaviours in temporary backup)
                // * unload domain
                // * load new domain
                // * OnEnable (Deserialize monobehaviours in temporary backup)
                // * mark entered playmode / load scene
                // * OnDisable / OnDestroy
                // * OnEnable (Loading object from scene...)
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    // We are just gonna ignore this enter playmode reload.
                    // Can't see a situation where it would be useful to create something inbetween.
                    // But we really need to solve this at the root. The execution order is kind if crazy.
                    if (!EditorApplication.isPlaying)
                        return;
                    
                    Debug.LogError("Loading GameObjectEntity in Playmode but there is no active World");
                    return;
                }
                else
                {
#if UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP
                    return;
#else
                    DefaultWorldInitialization.Initialize("Editor World", true);
#endif
                }
            }
            #endif

            EntityManager = World.Active.GetOrCreateManager<EntityManager>();
            Entity = AddToEntityManager(EntityManager, gameObject);
        }

        protected virtual void OnDisable()
        {
            if (EntityManager != null && EntityManager.IsCreated && EntityManager.Exists(Entity))
                EntityManager.DestroyEntity(Entity);

            EntityManager = null;
            Entity = new Entity();
        }

        public void CopyAllComponentsToEntity(EntityManager entityManager, Entity entity)
        {
            foreach (var wrapper in gameObject.GetComponents<ComponentDataWrapperBase>())
            {
                //@TODO: handle shared components and tag components
                var type = wrapper.GetComponentType();
                entityManager.AddComponent(entity, type);
                wrapper.UpdateComponentData(entityManager, entity);
            }
        }
    }
}
