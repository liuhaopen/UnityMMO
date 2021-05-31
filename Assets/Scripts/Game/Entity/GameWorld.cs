﻿using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

using Unity.Entities;
using UnityEditor;
using UnityEngine.Profiling;
using Unity.Collections;


// #if UNITY_EDITOR
// [InitializeOnLoad]
// // Class makes sure we have world when starting game in editor (GameObjectEntities in scene needs world in OnEnable)   
// class EditorWorldCreator
// {
//     static EditorWorldCreator()
//     {
//         EditorApplication.playModeStateChanged -= EditorApplicationOnPlayModeStateChanged;
//         EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;

//         CreateWorld();
//     }

//     private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange change)
//     {
//         if (change == PlayModeStateChange.ExitingEditMode) 
//             ShutdownWorld();
//     }

//     static void CreateWorld()
//     {
//         if(World.Active == null)
//             World.Active = new World("EditorWorld"); 
//     }

//     static void ShutdownWorld()
//     {
//         if (World.Active != null)
//         {
//             World.Active.Dispose();
//             World.Active = null;
//         }
//     }
// }
// #endif        



public struct DespawningEntity : IComponentData
{
}

[InternalBufferCapacity(16)]
public struct EntityGroupChildren : IBufferElementData
{
    public Entity entity;
}

[DisableAutoCreation]
public class DestroyDespawning : ComponentSystem
{
    EntityQuery Group;

    protected override void OnCreate()
    {
        base.OnCreate();
        Group = GetEntityQuery(typeof(DespawningEntity));
    }
    
    protected override void OnUpdate()
    {
        var entityArray = Group.ToEntityArray(Allocator.TempJob);
        for (var i = 0; i < entityArray.Length; i++)
        {
            PostUpdateCommands.DestroyEntity(entityArray[i]);
        }
        entityArray.Dispose();
    }
}

public class GameWorld
{
    
    // TODO (petera) this is kind of ugly. But very useful to look at worlds from outside for stats purposes...
    public static List<GameWorld> s_Worlds = new List<GameWorld>();

    // public GameTime worldTime;

    public int lastServerTick;   

    public float frameDuration
    {
        get { return m_frameDuration; }
        set { m_frameDuration = value; }
    }


    public double nextTickTime = 0;

    // SceneRoot can be used to organize crated gameobject in scene view. Is null in standalone.
    public GameObject SceneRoot
    {
        get { return m_sceneRoot; }
    }
    
    public GameWorld(string name = "world")
    {
        // if (gameobjectHierarchy.IntValue == 1)
        // {
        //     m_sceneRoot = new GameObject(name);
        //     GameObject.DontDestroyOnLoad(m_sceneRoot);
        // }

// #if UNITY_EDITOR
        // When running in editor the world could be created, so we silently keep it.
        m_ECSWorld = World.Active != null ? World.Active : new World(name); 
// #else
//         GameDebug.Assert(World.Active == null);
//         m_ECSWorld = new World(name);
// #endif        
        

        World.Active = m_ECSWorld;
        m_EntityManager = m_ECSWorld.EntityManager;

        GameDebug.Assert(m_EntityManager.IsCreated);

        // worldTime.tickRate = 60;

        // nextTickTime = Game.frameTime;

        s_Worlds.Add(this);

        m_destroyDespawningSystem = m_ECSWorld.CreateSystem<DestroyDespawning>();
    }

    public void Shutdown()
    {
        foreach (var entity in m_dynamicEntities)
        {
            if (m_DespawnRequests.Contains(entity))
                continue;

#if UNITY_EDITOR
            if (entity == null)
                continue;

            var gameObjectEntity = entity.GetComponent<GameObjectEntity>();
            if (gameObjectEntity != null && !m_EntityManager.Exists(gameObjectEntity.Entity))
                continue;
#endif            
            
            RequestDespawn(entity);
        }
        ProcessDespawns();

        s_Worlds.Remove(this);

        if (m_ECSWorld.IsCreated)
        {
            m_ECSWorld.Dispose();
            m_ECSWorld = null;
            World.Active = null;
        }

        GameObject.Destroy(m_sceneRoot);
    }

    // public void RegisterSceneEntities()
    // {
    //     // Replicated entities are sorted by their netID and numbered accordingly
    //     var sceneEntities = new List<ReplicatedEntity>(Object.FindObjectsOfType<ReplicatedEntity>());
    //     // sceneEntities.Sort((a, b) => ByteArrayComp.instance.Compare(a.netID, b.netID));
    //     for (int i = 0; i < sceneEntities.Count; i++)
    //     {
    //         GameDebug.Assert(sceneEntities[i].GetComponent<SceneEntity>() != null, "Entity {0} has replciated component but does not have scene entity",sceneEntities[i]);
    //         sceneEntities[i].id = i;
    //     }
    //     m_sceneEntities.AddRange(sceneEntities);
    // }
    
    public EntityManager GetEntityManager()    
    {
        return m_EntityManager;
    }

    public World GetECSWorld()    
    {
        return m_ECSWorld;
    }

    public T SpawnByGameObject<T>(GameObject gameObject) where T : Component
    {
        return SpawnByGameObject<T>(gameObject, Vector3.zero, Quaternion.identity);
    }

    public T SpawnByGameObject<T>(GameObject gameObject, Vector3 position, Quaternion rotation) where T : Component
    {
        Entity entity = RegisterInternal(gameObject, true);
        if (gameObject == null)
            return null;

        var result = gameObject.GetComponent<T>();
        if (result == null)
        {
            GameDebug.Log(string.Format("Spawned entity '{0}' didn't have component '{1}'", gameObject, typeof(T).FullName));
            return null;
        }

        return result;
    }

    public T Spawn<T>(GameObject prefab) where T : Component
    {
        return Spawn<T>(prefab, Vector3.zero, Quaternion.identity);
    }

    public T Spawn<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        Entity entity;
        var gameObject = SpawnInternal(prefab, position, rotation, out entity);
        if (gameObject == null)
            return null;

        var result = gameObject.GetComponent<T>();
        if (result == null)
        {
            GameDebug.Log(string.Format("Spawned entity '{0}' didn't have component '{1}'", prefab, typeof(T).FullName));
            return null;
        }

        return result;
    }
    
    public GameObject Spawn(string name, params System.Type[] components)
    {
        var go = new GameObject(name, components);
        RegisterInternal(go, true);
        return go;
    }

    public GameObject SpawnInternal(GameObject prefab, Vector3 position, Quaternion rotation, out Entity entity)
    {
        Profiler.BeginSample("GameWorld.SpawnInternal");
        
        var go = Object.Instantiate(prefab, position, rotation);

        entity = RegisterInternal(go, true);

        Profiler.EndSample();
        
        return go;
    }

    ////////////////////////////////////////////////////////////////////////////////

    // public List<ReplicatedEntity> SceneEntities { get { return m_sceneEntities; } }

    public void RequestDespawn(GameObject entity)
    {
        if (m_DespawnRequests.Contains(entity))
        {
            GameDebug.Assert(false, "Trying to request depawn of same gameobject({0}) multiple times",entity.name);
            return;
        }

        var gameObjectEntity = entity.GetComponent<GameObjectEntity>();
        if(gameObjectEntity != null)
            m_EntityManager.AddComponent(gameObjectEntity.Entity, typeof(DespawningEntity));
        
        m_DespawnRequests.Add(entity);
    }

    public void RequestDespawn(GameObject entity, EntityCommandBuffer commandBuffer)
    {
        if (m_DespawnRequests.Contains(entity))
        {
            GameDebug.Assert(false, "Trying to request depawn of same gameobject({0}) multiple times",entity.name);
            return;
        }

        var gameObjectEntity = entity.GetComponent<GameObjectEntity>();
        if(gameObjectEntity != null)
            commandBuffer.AddComponent(gameObjectEntity.Entity, new DespawningEntity());
        
        m_DespawnRequests.Add(entity);
    }

    public void RequestDespawn(Entity entity)
    {
        m_EntityManager.AddComponent(entity, typeof(DespawningEntity));
        m_DespawnEntityRequests.Add(entity);
        
        if (m_EntityManager.HasComponent<EntityGroupChildren>(entity))
        {
            // Copy buffer as we dont have EntityCommandBuffer to perform changes            
            var buffer = m_EntityManager.GetBuffer<EntityGroupChildren>(entity);
            var entities = new Entity[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                entities[i] = buffer[i].entity;
            }
            
            for (int i = 0; i < entities.Length; i++)
            {
                m_EntityManager.AddComponent(entities[i], typeof(DespawningEntity));
                m_DespawnEntityRequests.Add(entities[i]);
            }
        }
    }
    
    public void RequestDespawn(EntityCommandBuffer commandBuffer, Entity entity)
    {
        if (m_DespawnEntityRequests.Contains(entity))
        {
            GameDebug.Assert(false, "Trying to request depawn of same gameobject({0}) multiple times",entity);
            return;
        }
        commandBuffer.AddComponent(entity, new DespawningEntity());
        m_DespawnEntityRequests.Add(entity);

        if (m_EntityManager.HasComponent<EntityGroupChildren>(entity))
        {
            var buffer = m_EntityManager.GetBuffer<EntityGroupChildren>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                commandBuffer.AddComponent(buffer[i].entity, new DespawningEntity());
                m_DespawnEntityRequests.Add(buffer[i].entity);
            }
        }
    }
    
    public void ProcessDespawns()
    {
        foreach (var gameObject in m_DespawnRequests)
        {
            m_dynamicEntities.Remove(gameObject);
            Object.Destroy(gameObject);
        }

        foreach (var entity in m_DespawnEntityRequests)
        {
            m_EntityManager.DestroyEntity(entity);
        }
        m_DespawnEntityRequests.Clear();
        m_DespawnRequests.Clear();

        m_destroyDespawningSystem.Update();
    }

    Entity RegisterInternal(GameObject gameObject, bool isDynamic)
    {
        // If gameObject has GameObjectEntity it is already registered in entitymanager. If not we register it here  
        var gameObjectEntity = gameObject.GetComponent<GameObjectEntity>();
        if(gameObjectEntity == null)
            GameObjectEntity.AddToEntityManager(m_EntityManager, gameObject);
        
        if (isDynamic)
            m_dynamicEntities.Add(gameObject);

        return gameObjectEntity != null ? gameObjectEntity.Entity : Entity.Null;
    }

    EntityManager m_EntityManager;
    World m_ECSWorld;

    GameObject m_sceneRoot;

    DestroyDespawning m_destroyDespawningSystem;

    List<GameObject> m_dynamicEntities = new List<GameObject>();
    // List<ReplicatedEntity> m_sceneEntities = new List<ReplicatedEntity>();
    List<GameObject> m_DespawnRequests = new List<GameObject>(32);
    List<Entity> m_DespawnEntityRequests = new List<Entity>(32);

    // [ConfigVar(Name = "gameobjecthierarchy", Description = "Should gameobject be organized in a gameobject hierarchy", DefaultValue = "0")]
    // static ConfigVar gameobjectHierarchy;
    
    float m_frameDuration;
}
