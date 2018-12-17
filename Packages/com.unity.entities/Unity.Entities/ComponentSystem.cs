using System;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using System.Collections.Generic;

namespace Unity.Entities
{
    public unsafe abstract class ComponentSystemBase : ScriptBehaviourManager
    {
        InjectComponentGroupData[] 			m_InjectedComponentGroups;
        InjectFromEntityData                m_InjectFromEntityData;

        ComponentGroupArrayStaticCache[] 	m_CachedComponentGroupArrays;
        ComponentGroup[] 				    m_ComponentGroups;

        NativeList<int>                     m_JobDependencyForReadingManagers;
        NativeList<int>                     m_JobDependencyForWritingManagers;

        internal int*                       m_JobDependencyForReadingManagersPtr;
        internal int                        m_JobDependencyForReadingManagersLength;

        internal int*                       m_JobDependencyForWritingManagersPtr;
        internal int                        m_JobDependencyForWritingManagersLength;

        uint                                m_LastSystemVersion;
        
        internal ComponentJobSafetyManager  m_SafetyManager;
        internal EntityManager              m_EntityManager;
        World                               m_World;

        bool                                m_AlwaysUpdateSystem;
        internal bool                       m_PreviouslyEnabled;

        public bool Enabled { get; set; } = true;
        public ComponentGroup[] 			ComponentGroups => m_ComponentGroups;
        
        public uint GlobalSystemVersion => m_EntityManager.GlobalSystemVersion;
        public uint LastSystemVersion   => m_LastSystemVersion;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal int                        m_SystemID;
        static internal ComponentSystemBase ms_ExecutingSystem;

        internal ComponentSystemBase GetSystemFromSystemID(World world, int systemID)
        {
            foreach(var m in world.BehaviourManagers)
            {
                var system = m as ComponentSystemBase;
                if (system == null)
                    continue;
                if (system.m_SystemID == systemID)
                    return system;
            }

            return null;
        }
#endif
        
        public bool ShouldRunSystem()
        {
            if (!m_World.IsCreated)
                return false;

            if (m_AlwaysUpdateSystem)
                return true;

            var length = m_ComponentGroups?.Length ?? 0;

            if (length == 0)
                return true;

            // If all the groups are empty, skip it.
            // (There’s no way to know what they key value is without other markup)
            for (int i = 0;i != length;i++)
            {
                if (!m_ComponentGroups[i].IsEmptyIgnoreFilter)
                    return true;
            }

            return false;
        }

        protected override void OnBeforeCreateManagerInternal(World world)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_SystemID = world.AllocateSystemID();
#endif
            m_World = world;
            m_EntityManager = world.GetOrCreateManager<EntityManager>();
            m_SafetyManager = m_EntityManager.ComponentJobSafetyManager;
            m_AlwaysUpdateSystem = GetType().GetCustomAttributes(typeof(AlwaysUpdateSystemAttribute), true).Length != 0;

            m_ComponentGroups = new ComponentGroup[0];
            m_CachedComponentGroupArrays = new ComponentGroupArrayStaticCache[0];
            m_JobDependencyForReadingManagers = new NativeList<int>(10, Allocator.Persistent);
            m_JobDependencyForWritingManagers = new NativeList<int>(10, Allocator.Persistent);

            ComponentSystemInjection.Inject(this, world, m_EntityManager, out m_InjectedComponentGroups, out m_InjectFromEntityData);
            m_InjectFromEntityData.ExtractJobDependencyTypes(this);

            InjectNestedIJobProcessComponentDataJobs();

            UpdateInjectedComponentGroups();
        }

        void InjectNestedIJobProcessComponentDataJobs()
        {
            // Create ComponentGroup for all nested IJobProcessComponentData jobs
            foreach (var nestedType in GetType().GetNestedTypes(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public))
                JobProcessComponentDataExtensions.GetComponentGroupForIJobProcessComponentData(this, nestedType);
        }

        protected sealed override void OnAfterDestroyManagerInternal()
        {
            foreach (var group in m_ComponentGroups)
            {
                #if ENABLE_UNITY_COLLECTIONS_CHECKS
                group.DisallowDisposing = null;
                #endif
                group.Dispose();
            }
            m_ComponentGroups = null;
            m_InjectedComponentGroups = null;
            m_CachedComponentGroupArrays = null;

            if (m_JobDependencyForReadingManagers.IsCreated)
                m_JobDependencyForReadingManagers.Dispose();
            if (m_JobDependencyForWritingManagers.IsCreated)
                m_JobDependencyForWritingManagers.Dispose();
        }

        protected override void OnBeforeDestroyManagerInternal()
        {
            if (m_PreviouslyEnabled)
            {
                m_PreviouslyEnabled = false;
                OnStopRunning();
            }
            CompleteDependencyInternal();
            UpdateInjectedComponentGroups();
        }
        
        protected virtual void OnStartRunning()
        {

        }

        protected virtual void OnStopRunning()
        {

        }

        protected internal void BeforeUpdateVersioning()
        {
            m_EntityManager.Entities->IncrementGlobalSystemVersion();
            foreach (var group in m_ComponentGroups)
                group.SetFilterChangedRequiredVersion(m_LastSystemVersion);
        }

        protected internal void AfterUpdateVersioning()
        {
            m_LastSystemVersion = EntityManager.Entities->GlobalSystemVersion;
        }
        
        protected EntityManager EntityManager => m_EntityManager;
        protected World World => m_World;

        // TODO: this should be made part of UnityEngine?
        static void ArrayUtilityAdd<T>(ref T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }
        
        public ArchetypeChunkComponentType<T> GetArchetypeChunkComponentType<T>(bool isReadOnly = false)
            where T : struct, IComponentData
        {
            AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.Create<T>());
            return EntityManager.GetArchetypeChunkComponentType<T>(isReadOnly);
        }

        public ArchetypeChunkBufferType<T> GetArchetypeChunkBufferType<T>(bool isReadOnly = false)
            where T : struct, IBufferElementData
        {
            AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.Create<T>());
            return EntityManager.GetArchetypeChunkBufferType<T>(isReadOnly);
        }

        public ArchetypeChunkSharedComponentType<T> GetArchetypeChunkSharedComponentType<T>()
            where T : struct, ISharedComponentData
        {
            AddReaderWriter(ComponentType.ReadOnly<T>());
            return EntityManager.GetArchetypeChunkSharedComponentType<T>();
        }

        public ArchetypeChunkEntityType GetArchetypeChunkEntityType()
        {
            AddReaderWriter(ComponentType.ReadOnly<Entity>());
            return EntityManager.GetArchetypeChunkEntityType();
        }
        
        public ComponentDataFromEntity<T> GetComponentDataFromEntity<T>(bool isReadOnly = false)
            where T : struct, IComponentData
        {
            AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.Create<T>());
            return EntityManager.GetComponentDataFromEntity<T>(isReadOnly);
        }
        
        internal void AddReaderWriter(ComponentType componentType)
        {
            if (CalculateReaderWriterDependency.Add(componentType, m_JobDependencyForReadingManagers, m_JobDependencyForWritingManagers))
            {
                m_JobDependencyForReadingManagersPtr = (int*)m_JobDependencyForReadingManagers.GetUnsafePtr();
                m_JobDependencyForWritingManagersPtr = (int*)m_JobDependencyForWritingManagers.GetUnsafePtr();

                m_JobDependencyForReadingManagersLength = m_JobDependencyForReadingManagers.Length;
                m_JobDependencyForWritingManagersLength = m_JobDependencyForWritingManagers.Length;
                CompleteDependencyInternal();
            }
        }
        
        internal ComponentGroup GetComponentGroupInternal(ComponentType[] componentTypes)
        {
            for (var i = 0; i != m_ComponentGroups.Length; i++)
            {
                if (m_ComponentGroups[i].CompareComponents(componentTypes))
                    return m_ComponentGroups[i];
            }

            var group = EntityManager.CreateComponentGroup(componentTypes);
            for (int i = 0;i != componentTypes.Length;i++)
                AddReaderWriter(componentTypes[i]);

            AfterGroupCreated(group);
            
            return group;
        }

        internal ComponentGroup GetComponentGroupInternal(EntityArchetypeQuery[] query)
        {
            for (var i = 0; i != m_ComponentGroups.Length; i++)
            {
                if (m_ComponentGroups[i].CompareQuery(query))
                    return m_ComponentGroups[i];
            }

            var group = EntityManager.CreateComponentGroup(query);
            AfterGroupCreated(group);

            return group;
        }

        void AfterGroupCreated(ComponentGroup group)
        {
            group.SetFilterChangedRequiredVersion(m_LastSystemVersion);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            group.DisallowDisposing = "ComponentGroup.Dispose() may not be called on a ComponentGroup created with ComponentSystem.GetComponentGroup. The ComponentGroup will automatically be disposed by the ComponentSystem.";
#endif
            
            ArrayUtilityAdd(ref m_ComponentGroups, group);
        }

        protected ComponentGroup GetComponentGroup(params ComponentType[] componentTypes)
        {
            return GetComponentGroupInternal(componentTypes);
        }

        protected ComponentGroup GetComponentGroup(params EntityArchetypeQuery[] query)
        {
            return GetComponentGroupInternal(query);
        }
        
        protected ComponentGroupArray<T> GetEntities<T>() where T : struct
        {
            for (var i = 0; i != m_CachedComponentGroupArrays.Length; i++)
            {
                if (m_CachedComponentGroupArrays[i].CachedType == typeof(T))
                    return new ComponentGroupArray<T>(m_CachedComponentGroupArrays[i]);
            }

            var cache = new ComponentGroupArrayStaticCache(typeof(T), EntityManager, this);
            ArrayUtilityAdd(ref m_CachedComponentGroupArrays, cache);
            return new ComponentGroupArray<T>(cache);
        }

        protected void UpdateInjectedComponentGroups()
        {
            if (null == m_InjectedComponentGroups)
                return;

            ulong gchandle;
            var pinnedSystemPtr = (byte*)UnsafeUtility.PinGCObjectAndGetAddress(this, out gchandle);

            try
            {
                foreach (var group in m_InjectedComponentGroups)
                    group.UpdateInjection (pinnedSystemPtr);

                m_InjectFromEntityData.UpdateInjection(pinnedSystemPtr, EntityManager);
            }
            catch
            {
                UnsafeUtility.ReleaseGCObject(gchandle);
                throw;
            }
            UnsafeUtility.ReleaseGCObject(gchandle);
        }

        internal void CompleteDependencyInternal()
        {
            m_SafetyManager.CompleteDependenciesNoChecks(m_JobDependencyForReadingManagersPtr, m_JobDependencyForReadingManagersLength, m_JobDependencyForWritingManagersPtr, m_JobDependencyForWritingManagersLength);
        }
    }

    public abstract class ComponentSystem : ComponentSystemBase
    {
        EntityCommandBuffer m_DeferredEntities;

        public EntityCommandBuffer PostUpdateCommands => m_DeferredEntities;

        unsafe void BeforeOnUpdate()
        {
            BeforeUpdateVersioning();
            CompleteDependencyInternal();
            UpdateInjectedComponentGroups();

            m_DeferredEntities = new EntityCommandBuffer(Allocator.TempJob);
        }

        void AfterOnUpdate()
        {
            AfterUpdateVersioning();

            JobHandle.ScheduleBatchedJobs();

#if ENABLE_UNITY_COLLECTIONS_CHECKS	
            try
            {
                m_DeferredEntities.Playback(EntityManager);
            }
            catch (Exception e)
            {
                m_DeferredEntities.Dispose();
                var error = $"{e.Message}\nEntityCommandBuffer was recorded in {GetType()} using PostUpdateCommands.\n" + e.StackTrace; 
                throw new System.ArgumentException(error);
            }
#else
            m_DeferredEntities.Playback(EntityManager);
#endif
            m_DeferredEntities.Dispose();            
        }

        internal sealed override void InternalUpdate()
        {
            if (Enabled && ShouldRunSystem())
            {
                if (!m_PreviouslyEnabled)
                {
                    m_PreviouslyEnabled = true;
                    OnStartRunning();
                }

                BeforeOnUpdate();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                var oldExecutingSystem = ms_ExecutingSystem; 
                ms_ExecutingSystem = this;
#endif

                try
                {
                    OnUpdate();
                }
                finally
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS                    
                    ms_ExecutingSystem = oldExecutingSystem;
#endif
                    AfterOnUpdate();
                }
            }
            else if (m_PreviouslyEnabled)
            {
                m_PreviouslyEnabled = false;
                OnStopRunning();
            }
        }

        protected sealed override void OnBeforeCreateManagerInternal(World world)
        {
            base.OnBeforeCreateManagerInternal(world);
        }

        protected sealed override void OnBeforeDestroyManagerInternal()
        {
            base.OnBeforeDestroyManagerInternal();
        }

        /// <summary>
        /// Called once per frame on the main thread.
        /// </summary>
        protected abstract void OnUpdate();
}

    public abstract class JobComponentSystem : ComponentSystemBase
    {
        JobHandle m_PreviousFrameDependency;
        BarrierSystem[] m_BarrierList;

        unsafe JobHandle BeforeOnUpdate()
        {
            BeforeUpdateVersioning();

            UpdateInjectedComponentGroups();

            // We need to wait on all previous frame dependencies, otherwise it is possible that we create infinitely long dependency chains
            // without anyone ever waiting on it
            m_PreviousFrameDependency.Complete();

            return GetDependency();
        }

        unsafe void AfterOnUpdate(JobHandle outputJob, bool throwException)
        {
            AfterUpdateVersioning();
            
            JobHandle.ScheduleBatchedJobs();

            AddDependencyInternal(outputJob);

            // Notify all injected barrier systems that they will need to sync on any jobs we spawned.
            // This is conservative currently - the barriers will sync on too much if we use more than one.
            for (int i = 0; i < m_BarrierList.Length; ++i)
            {
                m_BarrierList[i].AddJobHandleForProducer(outputJob);
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS

            if (!JobsUtility.JobDebuggerEnabled)
                return;

            // Check that all reading and writing jobs are a dependency of the output job, to
            // catch systems that forget to add one of their jobs to the dependency graph.
            //
            // Note that this check is not strictly needed as we would catch the mistake anyway later,
            // but checking it here means we can flag the system that has the mistake, rather than some
            // other (innocent) system that is doing things correctly.

            //@TODO: It is not ideal that we call m_SafetyManager.GetDependency,
            //       it can result in JobHandle.CombineDependencies calls.
            //       Which seems like debug code might have side-effects

            string dependencyError = null;
            for (var index = 0; index < m_JobDependencyForReadingManagersLength && dependencyError == null; index++)
            {
                var type = m_JobDependencyForReadingManagersPtr[index];
                dependencyError = CheckJobDependencies(type);
            }

            for (var index = 0; index < m_JobDependencyForWritingManagersLength && dependencyError == null; index++)
            {
                var type = m_JobDependencyForWritingManagersPtr[index];
                dependencyError = CheckJobDependencies(type);
            }

            if (dependencyError != null)
            {
                EmergencySyncAllJobs();

                if (throwException)
                    throw new System.InvalidOperationException(dependencyError);
            }
#endif
        }

        internal sealed override void InternalUpdate()
        {
            if (Enabled && ShouldRunSystem())
            {
                if (!m_PreviouslyEnabled)
                {
                    m_PreviouslyEnabled = true;
                    OnStartRunning();
                }

                var inputJob = BeforeOnUpdate();
                JobHandle outputJob = new JobHandle();
                
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                var oldExecutingSystem = ms_ExecutingSystem; 
                ms_ExecutingSystem = this;
#endif
                try
                {
                    outputJob = OnUpdate(inputJob);
                }
                catch
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    ms_ExecutingSystem = oldExecutingSystem;
#endif

                    AfterOnUpdate(outputJob, false);
                    throw;
                }

                AfterOnUpdate(outputJob, true);
            }
            else if (m_PreviouslyEnabled)
            {
                m_PreviouslyEnabled = false;
                OnStopRunning();
            }
        }

        protected sealed override void OnBeforeCreateManagerInternal(World world)
        {
            base.OnBeforeCreateManagerInternal(world);

            m_BarrierList = ComponentSystemInjection.GetAllInjectedManagers<BarrierSystem>(this, world);
        }

        protected sealed override void OnBeforeDestroyManagerInternal()
        {
            base.OnBeforeDestroyManagerInternal();
            m_PreviousFrameDependency.Complete();
        }

        public BufferFromEntity<T> GetBufferFromEntity<T>(bool isReadOnly = false) where T : struct, IBufferElementData
        {
            AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.Create<T>());
            return EntityManager.GetBufferFromEntity<T>(isReadOnly);
        }

        protected virtual JobHandle OnUpdate(JobHandle inputDeps)
        {
            return inputDeps;
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        unsafe string CheckJobDependencies(int type)
        {
            var h = m_SafetyManager.GetSafetyHandle(type, true);

            var readerCount = AtomicSafetyHandle.GetReaderArray(h, 0, IntPtr.Zero);
            JobHandle* readers = stackalloc JobHandle[readerCount];
            AtomicSafetyHandle.GetReaderArray(h, readerCount, (IntPtr) readers);

            for (var i = 0; i < readerCount; ++i)
            {
                if (!m_SafetyManager.HasReaderOrWriterDependency(type, readers[i]))
                    return $"The system {GetType()} reads {TypeManager.GetType(type)} via {AtomicSafetyHandle.GetReaderName(h, i)} but that type was not returned as a job dependency. To ensure correct behavior of other systems, the job or a dependency of it must be returned from the OnUpdate method.";
            }

            if (!m_SafetyManager.HasReaderOrWriterDependency(type, AtomicSafetyHandle.GetWriter(h)))
                return $"The system {GetType()} writes {TypeManager.GetType(type)} via {AtomicSafetyHandle.GetWriterName(h)} but that was not returned as a job dependency. To ensure correct behavior of other systems, the job or a dependency of it must be returned from the OnUpdate method.";

            return null;
        }

        unsafe void EmergencySyncAllJobs()
        {
            for (int i = 0;i != m_JobDependencyForReadingManagersLength;i++)
            {
                int type = m_JobDependencyForReadingManagersPtr[i];
                AtomicSafetyHandle.EnforceAllBufferJobsHaveCompleted(m_SafetyManager.GetSafetyHandle(type, true));
            }

            for (int i = 0;i != m_JobDependencyForWritingManagersLength;i++)
            {
                int type = m_JobDependencyForWritingManagersPtr[i];
                AtomicSafetyHandle.EnforceAllBufferJobsHaveCompleted(m_SafetyManager.GetSafetyHandle(type, true));
            }
        }
#endif

        unsafe JobHandle GetDependency ()
        {
            return m_SafetyManager.GetDependency(m_JobDependencyForReadingManagersPtr, m_JobDependencyForReadingManagersLength, m_JobDependencyForWritingManagersPtr, m_JobDependencyForWritingManagersLength);
        }

        unsafe void AddDependencyInternal(JobHandle dependency)
        {
            m_PreviousFrameDependency = m_SafetyManager.AddDependency(m_JobDependencyForReadingManagersPtr, m_JobDependencyForReadingManagersLength, m_JobDependencyForWritingManagersPtr, m_JobDependencyForWritingManagersLength, dependency);
        }
    }

    public unsafe abstract class BarrierSystem : ComponentSystem
    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS	
        private List<EntityCommandBuffer> m_PendingBuffers;	
#else	
        private NativeList<EntityCommandBuffer> m_PendingBuffers;
#endif
        private JobHandle m_ProducerHandle;

        public EntityCommandBuffer CreateCommandBuffer()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            cmds.SystemID = ms_ExecutingSystem != null ? ms_ExecutingSystem.m_SystemID : 0;  
#endif
            m_PendingBuffers.Add(cmds);

            return cmds;
        }

        internal void AddJobHandleForProducer(JobHandle foo)
        {
            m_ProducerHandle = JobHandle.CombineDependencies(m_ProducerHandle, foo);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS	
            m_PendingBuffers = new List<EntityCommandBuffer>();	
#else	
            m_PendingBuffers = new NativeList<EntityCommandBuffer>(Allocator.Persistent);
#endif
        }

        protected override void OnDestroyManager()
        {
            FlushBuffers(false);

#if !ENABLE_UNITY_COLLECTIONS_CHECKS
            m_PendingBuffers.Dispose();
#endif

            base.OnDestroyManager();
        }

        protected sealed override void OnUpdate()
        {
            FlushBuffers(true);
        }

        private void FlushBuffers(bool playBack)
        {
            m_ProducerHandle.Complete();
            m_ProducerHandle = new JobHandle();

            int length;
#if ENABLE_UNITY_COLLECTIONS_CHECKS	
            length = m_PendingBuffers.Count;	
#else	
            length = m_PendingBuffers.Length;	
#endif
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS	
            Exception exception = null;
#endif            
            for (int i = 0; i < length; ++i)
            {
                var buffer = m_PendingBuffers[i];
                if (playBack)
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS	
                    try
                    {
                        buffer.Playback(EntityManager);
                    }
                    catch (Exception e)
                    {
                        
                        var system = GetSystemFromSystemID(World, buffer.SystemID);
                        var systemType = system != null ? system.GetType().ToString() : "Unknown";
                                                
                        var error = $"{e.Message}\nEntityCommandBuffer was recorded in {systemType} and played back in {GetType()}.\n" + e.StackTrace; 
                        exception = new System.ArgumentException(error);
                    }
#else
                    buffer.Playback(EntityManager);
#endif
                }
                buffer.Dispose();
            }
            m_PendingBuffers.Clear();

#if ENABLE_UNITY_COLLECTIONS_CHECKS	
            if (exception != null)
                throw exception;
#endif
        }
    }
}
