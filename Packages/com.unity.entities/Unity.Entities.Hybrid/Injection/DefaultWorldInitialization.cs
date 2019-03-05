using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Unity.Entities
{
    public static class DefaultWorldInitialization
    {
        static void DomainUnloadShutdown()
        {
            World.DisposeAllWorlds();

            WordStorage.Instance.Dispose();
            WordStorage.Instance = null;
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
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


        public static void Initialize(string worldName, bool editorWorld)
        {
            var world = new World(worldName);
            World.Active = world;

            // Register hybrid injection hooks
            InjectionHookSupport.RegisterHook(new GameObjectArrayInjectionHook());
            InjectionHookSupport.RegisterHook(new TransformAccessArrayInjectionHook());
            InjectionHookSupport.RegisterHook(new ComponentArrayInjectionHook());

            PlayerLoopManager.RegisterDomainUnload(DomainUnloadShutdown, 10000);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!TypeManager.IsAssemblyReferencingEntities(assembly))
                    continue;
                
                try
                {
                    var allTypes = assembly.GetTypes();

                    // Create all ComponentSystem
                    CreateBehaviourManagersForMatchingTypes(editorWorld, allTypes, world);
                }
                catch
                {
                    Debug.LogWarning($"DefaultWorldInitialization failed loading assembly: {(assembly.IsDynamic ? assembly.ToString() : assembly.Location)}");
                }
            }
            
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(world);
        }

        static void CreateBehaviourManagersForMatchingTypes(bool editorWorld, IEnumerable<Type> allTypes, World world)
        {
            var systemTypes = allTypes.Where(t =>
                t.IsSubclassOf(typeof(ComponentSystemBase)) &&
                !t.IsAbstract &&
                !t.ContainsGenericParameters &&
                t.GetCustomAttributes(typeof(DisableAutoCreationAttribute), true).Length == 0 &&
                t.GetCustomAttributes(typeof(GameObjectToEntityConversionAttribute), true).Length == 0);
            
            foreach (var type in systemTypes)
            {
                if (editorWorld)
                {
                    if (Attribute.IsDefined(type, typeof(ExecuteInEditMode)))
                        Debug.LogError($"{type} is decorated with {typeof(ExecuteInEditMode)}. Support for this attribute will be deprecated. Please use {typeof(ExecuteAlways)} instead.");
                    else if (!Attribute.IsDefined(type, typeof(ExecuteAlways)))
                        continue;
                }

                GetBehaviourManagerAndLogException(world, type);
            }
        }
    }
}
