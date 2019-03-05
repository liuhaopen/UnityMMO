using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using GameObject = UnityEngine.GameObject;
using Component = UnityEngine.Component;

namespace Unity.Entities
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class GameObjectEntity : MonoBehaviour
    {
        public EntityManager EntityManager
        {
            get
            {
                if (enabled && gameObject.activeInHierarchy)
                    ReInitializeEntityManagerAndEntityIfNecessary();
                return m_EntityManager;
            }
        }
        EntityManager m_EntityManager;

        public Entity Entity
        {
            get
            {
                if (enabled && gameObject.activeInHierarchy)
                    ReInitializeEntityManagerAndEntityIfNecessary();
                return m_Entity;
            }
        }
        Entity m_Entity;

        void ReInitializeEntityManagerAndEntityIfNecessary()
        {
            // in case e.g., on a prefab that was open for edit when domain was unloaded
            // existing m_EntityManager lost all its data, so simply create a new one
            if (m_EntityManager != null && !m_EntityManager.IsCreated && !m_Entity.Equals(default(Entity)))
                Initialize();
        }

        // TODO: Very wrong error messages when creating entity with empty ComponentType array?
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
            Initialize();
        }

        void Initialize()
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

            m_EntityManager = World.Active.GetOrCreateManager<EntityManager>();
            m_Entity = AddToEntityManager(m_EntityManager, gameObject);
        }

        protected virtual void OnDisable()
        {
            if (EntityManager != null && EntityManager.IsCreated && EntityManager.Exists(Entity))
                EntityManager.DestroyEntity(Entity);

            m_EntityManager = null;
            m_Entity = Entity.Null;
        }

        public static void CopyAllComponentsToEntity(GameObject gameObject, EntityManager entityManager, Entity entity)
        {
            foreach (var wrapper in gameObject.GetComponents<ComponentDataWrapperBase>())
            {
                // TODO: handle shared components and tag components
                var type = wrapper.GetComponentType();
                entityManager.AddComponent(entity, type);
                wrapper.UpdateComponentData(entityManager, entity);
            }
        }
    }
}
