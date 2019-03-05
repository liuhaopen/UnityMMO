using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 162


public static class GameObjectConversionUtility
{
    public static void ConvertScene(Scene scene, World dstEntityWorld)
    {
        ConvertInternal(scene, dstEntityWorld, false);
    }

    public static void ConvertSceneAndApplyDiff(Scene scene, World previousStateShadowWorld, World dstEntityWorld)
    {
        using (var cleanConvertedEntityWorld = new World("Clean Entity Conversion World"))
        {
            ConvertInternal(scene, cleanConvertedEntityWorld, true);

            using (var diff = WorldDiffer.UpdateDiff(cleanConvertedEntityWorld, previousStateShadowWorld, Allocator.TempJob))
            {
                WorldDiffer.ApplyDiff(dstEntityWorld, diff);
            }
        }
    }

    static void ConvertInternal(Scene scene, World dstEntityWorld, bool addEntityGUID)
    {        
        using (var gameObjectWorld = new World("GameObject World"))
        {
            var mappingSystem = gameObjectWorld.CreateManager<GameObjectConversionMappingSystem>(dstEntityWorld);
            mappingSystem.AddEntityGUID = addEntityGUID;
            AddConversionSystems(gameObjectWorld);
    
            // Create Entities from game objects
            GameObjectConversionMappingSystem.CreateEntitiesForGameObjects(scene, gameObjectWorld);
            
            // Convert all the data into dstEntityWorld
            var managers = gameObjectWorld.BehaviourManagers;
            foreach (var manager in managers)
                manager.Update();
    
            mappingSystem.AddPrefabComponentDataTag();
        }
    }
    
    static void AddConversionSystems(World gameObjectWorld)
    {
        // Ensure the following systems run first in this order...
        gameObjectWorld.GetOrCreateManager<ConvertGameObjectToEntitySystemDeclarePrefabs>();
        gameObjectWorld.GetOrCreateManager<ConvertGameObjectToEntitySystem>();
        gameObjectWorld.GetOrCreateManager<ComponentDataWrapperToEntitySystem>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!TypeManager.IsAssemblyReferencingEntities(assembly))
                continue;
            
            try
            {
                var allTypes = assembly.GetTypes();
                CreateBehaviourManagersForMatchingTypes(allTypes, gameObjectWorld);
            }
            catch (ReflectionTypeLoadException)
            {
                Debug.LogWarning($"DefaultWorldInitialization failed loading assembly: {(assembly.IsDynamic ? assembly.ToString() : assembly.Location)}");
                continue;
            }
        }
    }

    static void CreateBehaviourManagersForMatchingTypes(IEnumerable<Type> allTypes, World world)
    {
        var systemTypes = allTypes.Where(t =>
            t.IsSubclassOf(typeof(ComponentSystemBase)) &&
            !t.IsAbstract &&
            !t.ContainsGenericParameters &&
            t.GetCustomAttributes(typeof(DisableAutoCreationAttribute), true).Length == 0 &&
            t.GetCustomAttributes(typeof(GameObjectToEntityConversionAttribute), true).Length != 0);

        foreach (var type in systemTypes)
        {
            GetBehaviourManagerAndLogException(world, type);
        }
    }
    
    static void GetBehaviourManagerAndLogException(World world, Type type)
    {
        try
        {
            world.GetOrCreateManager(type);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}