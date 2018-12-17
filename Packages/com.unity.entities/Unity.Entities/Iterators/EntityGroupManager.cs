using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace Unity.Entities
{
    internal unsafe class EntityGroupManager : IDisposable
    {
        private readonly ComponentJobSafetyManager m_JobSafetyManager;
        private ChunkAllocator m_GroupDataChunkAllocator;
        private EntityGroupData* m_LastGroupData;

        public EntityGroupManager(ComponentJobSafetyManager safetyManager)
        {
            m_JobSafetyManager = safetyManager;
        }

        public void Dispose()
        {
            //@TODO: Need to wait for all job handles to be completed..
            m_GroupDataChunkAllocator.Dispose();
        }

        ArchetypeQuery* CreateQuery(ComponentType[] requiredTypes)
        {
            var filter = (ArchetypeQuery*)m_GroupDataChunkAllocator.Allocate(sizeof(ArchetypeQuery), UnsafeUtility.AlignOf<ArchetypeQuery>());

            int noneCount = 0;
            int allCount = 0;
            for (int i = 0; i != requiredTypes.Length; i++)
            {
                if (requiredTypes[i].AccessModeType == ComponentType.AccessMode.Subtractive)
                    noneCount++;
                else
                    allCount++;
            }

            filter->All = (int*)m_GroupDataChunkAllocator.Allocate(sizeof(int) * allCount, UnsafeUtility.AlignOf<int>());
            filter->AllCount = allCount;

            filter->None = (int*)m_GroupDataChunkAllocator.Allocate(sizeof(int) * noneCount, UnsafeUtility.AlignOf<int>());
            filter->NoneCount = noneCount;

            filter->Any = null;
            filter->AnyCount = 0;

            noneCount = 0;
            allCount = 0;
            for (int i = 0; i != requiredTypes.Length; i++)
            {
                if (requiredTypes[i].AccessModeType == ComponentType.AccessMode.Subtractive)
                    filter->None[noneCount++] = requiredTypes[i].TypeIndex;
                else
                    filter->All[allCount++] = requiredTypes[i].TypeIndex;
            }
            
            return filter;
        }

        void ConstructTypeArray(ComponentType[] types, out int* outTypes, out int outLength)
        {
            if (types == null || types.Length == 0)
            {
                outTypes = null;
                outLength = 0;
            }
            else
            {
                outLength = types.Length;
                outTypes = (int*)m_GroupDataChunkAllocator.Allocate(sizeof(int) * types.Length, UnsafeUtility.AlignOf<int>());
                for (int i = 0; i != types.Length; i++)
                    outTypes[i] = types[i].TypeIndex;
            }
        }
        
        ArchetypeQuery* CreateQuery(EntityArchetypeQuery[] query)
        {
            //@TODO: Check that query doesn't contain any SubtractiveComponent...
            
            var outQuery = (ArchetypeQuery*)m_GroupDataChunkAllocator.Allocate(sizeof(ArchetypeQuery) * query.Length, UnsafeUtility.AlignOf<ArchetypeQuery>());
            for (int q = 0; q != query.Length; q++)
            {
                ConstructTypeArray(query[q].None, out outQuery[q].None, out outQuery[q].NoneCount);
                ConstructTypeArray(query[q].All,  out outQuery[q].All,  out outQuery[q].AllCount);
                ConstructTypeArray(query[q].Any,  out outQuery[q].Any,  out outQuery[q].AnyCount);
            }

            return outQuery;
        }
        
        void CreateRequiredComponents(ComponentType[] requiredComponents, out ComponentType* types, out int typesCount)
        {
            types = (ComponentType*) m_GroupDataChunkAllocator.Allocate(sizeof(ComponentType) * (requiredComponents.Length + 1), UnsafeUtility.AlignOf<ComponentType>());
            types[0] = ComponentType.Create<Entity>();
            for (int i = 0; i != requiredComponents.Length; i++)
                types[i + 1] = requiredComponents[i];
                    
            typesCount = requiredComponents.Length + 1;
        }
        
        public static bool CompareQueryArray(ComponentType[] filter, int* typeArray, int typeArrayCount)
        {
            int filterLength = filter != null ? filter.Length : 0;
            if (typeArrayCount != filterLength)
                return false;
            
            for (var i = 0; i < filterLength; ++i)
            {
                if (typeArray[i] != filter[i].TypeIndex)
                    return false;
            }

            return true;
        }

        public static bool CompareQuery(EntityArchetypeQuery[] query, EntityGroupData* groupData)
        {
            if (groupData->RequiredComponents != null)
                return false;
            
            if (groupData->ArchetypeQueryCount != query.Length)
                return false;

            for (int i = 0; i != query.Length; i++)
            {
                if (!CompareQueryArray(query[i].All, groupData->ArchetypeQuery[i].All, groupData->ArchetypeQuery[i].AllCount))
                    return false;
                if (!CompareQueryArray(query[i].None, groupData->ArchetypeQuery[i].None, groupData->ArchetypeQuery[i].NoneCount))
                    return false;
                if (!CompareQueryArray(query[i].Any, groupData->ArchetypeQuery[i].Any, groupData->ArchetypeQuery[i].AnyCount))
                    return false;
            }

            return true;
        }

        public static bool CompareComponents(ComponentType[] componentTypes, EntityGroupData* groupData)
        {
            if (groupData->RequiredComponents == null)
                return false;
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            for (var k = 0; k < componentTypes.Length; ++k)
                if (componentTypes[k].TypeIndex == TypeManager.GetTypeIndex<Entity>())
                    throw new ArgumentException(
                        "ComponentGroup.CompareComponents may not include typeof(Entity), it is implicit");
#endif

            // ComponentGroups are constructed including the Entity ID
            if (componentTypes.Length + 1 != groupData->RequiredComponentsCount)
                return false;

            for (var i = 0; i < componentTypes.Length; ++i)
            {
                if (groupData->RequiredComponents[i + 1] != componentTypes[i])
                    return false;
            }

            return true;
        }
        
        
        public ComponentGroup CreateEntityGroup(ArchetypeManager typeMan, EntityDataManager* entityDataManager, EntityArchetypeQuery[] query)
        {
            //@TODO: Support for CreateEntityGroup with query but using ComponentDataArray etc
            return CreateEntityGroup(typeMan, entityDataManager, CreateQuery(query), query.Length, null, 0);
        }

        public ComponentGroup CreateEntityGroup(ArchetypeManager typeMan, EntityDataManager* entityDataManager,
            ComponentType[] requiredComponents)
        {
            ComponentType* requiredComponentPtr;
            int requiredComponentCount;
            CreateRequiredComponents(requiredComponents, out requiredComponentPtr, out requiredComponentCount);
            return CreateEntityGroup(typeMan, entityDataManager, CreateQuery(requiredComponents), 1, requiredComponentPtr, requiredComponentCount);
        }

        public ComponentGroup CreateEntityGroup(ArchetypeManager typeMan, EntityDataManager* entityDataManager,
            ArchetypeQuery* archetypeQueries, int archetypeFiltersCount, ComponentType* requiredComponents, int requiredComponentsCount)
        {
            //@TODO: Validate that required types is subset of archetype filters all...
            
            var grp = (EntityGroupData*) m_GroupDataChunkAllocator.Allocate(sizeof(EntityGroupData), 8);
            grp->PrevGroup = m_LastGroupData;
            m_LastGroupData = grp;
            grp->RequiredComponentsCount = requiredComponentsCount;
            grp->RequiredComponents = requiredComponents;
            InitializeReaderWriter(grp, requiredComponents, requiredComponentsCount);
            
            grp->ArchetypeQuery = archetypeQueries;
            grp->ArchetypeQueryCount = archetypeFiltersCount;
            grp->FirstMatchingArchetype = null;
            grp->LastMatchingArchetype = null;
            for (var type = typeMan.m_LastArchetype; type != null; type = type->PrevArchetype)
                AddArchetypeIfMatching(type, grp);

            return new ComponentGroup(grp, m_JobSafetyManager, typeMan, entityDataManager);
        }
        
        void InitializeReaderWriter(EntityGroupData* grp, ComponentType* requiredTypes, int requiredCount)
        {
            grp->ReaderTypesCount = 0;
            grp->WriterTypesCount = 0;

            for (var i = 0; i != requiredCount; i++)
            {
                //@TODO: Investigate why Entity is not early out on this one...
                if (!requiredTypes[i].RequiresJobDependency)
                    continue;
                
                switch (requiredTypes[i].AccessModeType)
                {
                    case ComponentType.AccessMode.ReadOnly:
                        grp->ReaderTypesCount++;
                        break;
                    default:
                        grp->WriterTypesCount++;
                        break;
                }
            }

            grp->ReaderTypes = (int*) m_GroupDataChunkAllocator.Allocate(sizeof(int) * grp->ReaderTypesCount, 4);
            grp->WriterTypes = (int*) m_GroupDataChunkAllocator.Allocate(sizeof(int) * grp->WriterTypesCount, 4);

            var curReader = 0;
            var curWriter = 0;
            for (var i = 0; i != requiredCount; i++)
            {
                if (!requiredTypes[i].RequiresJobDependency)
                    continue;
                switch (requiredTypes[i].AccessModeType)
                {
                    case ComponentType.AccessMode.ReadOnly:
                        grp->ReaderTypes[curReader++] = requiredTypes[i].TypeIndex;
                        break;
                    default:
                        grp->WriterTypes[curWriter++] = requiredTypes[i].TypeIndex;
                        break;
                }
            }
        }

        public void AddArchetypeIfMatching(Archetype* type)
        {
            for (var grp = m_LastGroupData; grp != null; grp = grp->PrevGroup)
                AddArchetypeIfMatching(type, grp);
        }

        void AddArchetypeIfMatching(Archetype* archetype, EntityGroupData* group)
        {
            if (!IsMatchingArchetype(archetype, group))
                return;
            
            var match = (MatchingArchetypes*) m_GroupDataChunkAllocator.Allocate(
                MatchingArchetypes.GetAllocationSize(group->RequiredComponentsCount), 8);
            match->Archetype = archetype;
            var typeIndexInArchetypeArray = match->IndexInArchetype;

            if (group->LastMatchingArchetype == null)
                group->LastMatchingArchetype = match;

            match->Next = group->FirstMatchingArchetype;
            group->FirstMatchingArchetype = match;

            for (var component = 0; component < group->RequiredComponentsCount; ++component)
            {
                var typeComponentIndex = -1;
                if (group->RequiredComponents[component].AccessModeType != ComponentType.AccessMode.Subtractive)
                {
                    typeComponentIndex = ChunkDataUtility.GetIndexInTypeArray(archetype, group->RequiredComponents[component].TypeIndex);
                    Assert.AreNotEqual(-1, typeComponentIndex);
                }

                typeIndexInArchetypeArray[component] = typeComponentIndex;
            }
        }

        
        //@TODO: All this could be much faster by having all ComponentType pre-sorted to perform a single search loop instead two nested for loops... 
        static bool IsMatchingArchetype(Archetype* archetype, EntityGroupData* group)
        {
            for (int i = 0; i != group->ArchetypeQueryCount; i++)
            {
                if (IsMatchingArchetype(archetype, group->ArchetypeQuery + i))
                    return true;
            }

            return false;
        }

        static bool IsMatchingArchetype(Archetype* archetype, ArchetypeQuery* query)
        {
            if (!TestMatchingArchetypeAll(archetype, query->All, query->AllCount))
                return false;
            if (!TestMatchingArchetypeNone(archetype, query->None, query->NoneCount))
                return false;
            if (!TestMatchingArchetypeAny(archetype, query->Any, query->AnyCount))
                return false;

            return true;
        }
        
        static bool TestMatchingArchetypeAny(Archetype* archetype, int* anyTypes, int anyCount)
        {
            if (anyCount == 0) return true;

            var componentTypes = archetype->Types;
            var componentTypesCount = archetype->TypesCount;
            for (var i = 0; i < componentTypesCount; i++)
            {
                var componentTypeIndex = componentTypes[i].TypeIndex;
                for (var j = 0; j < anyCount; j++)
                {
                    var anyTypeIndex = anyTypes[j];
                    if (componentTypeIndex == anyTypeIndex) 
                        return true;
                }
            }

            return false;
        }
        
        static bool TestMatchingArchetypeNone(Archetype* archetype, int* noneTypes, int noneCount)
        {
            var componentTypes = archetype->Types;
            var componentTypesCount = archetype->TypesCount;
            for (var i = 0; i < componentTypesCount; i++)
            {
                var componentTypeIndex = componentTypes[i].TypeIndex;
                for (var j = 0; j < noneCount; j++)
                {
                    var noneTypeIndex = noneTypes[j];
                    if (componentTypeIndex == noneTypeIndex) return false;
                }
            }

            return true;
        }

        static bool TestMatchingArchetypeAll(Archetype* archetype, int* allTypes, int allCount)
        {
            var componentTypes = archetype->Types;
            var componentTypesCount = archetype->TypesCount;
            var foundCount = 0;
            var disabledTypeIndex = TypeManager.GetTypeIndex<Disabled>();
            var prefabTypeIndex = TypeManager.GetTypeIndex<Prefab>();
            var requestedDisabled = false;
            var requestedPrefab = false;
            for (var i = 0; i < componentTypesCount; i++)
            {
                var componentTypeIndex = componentTypes[i].TypeIndex;
                for (var j = 0; j < allCount; j++)
                {
                    var allTypeIndex = allTypes[j];
                    if (allTypeIndex == disabledTypeIndex)
                        requestedDisabled = true;
                    if (allTypeIndex == prefabTypeIndex)
                        requestedPrefab = true;
                    if (componentTypeIndex == allTypeIndex) foundCount++;
                }
            }

            if (archetype->Disabled && (!requestedDisabled))
                return false;
            if (archetype->Prefab && (!requestedPrefab))
                return false;

            return foundCount == allCount;
        }
    }

    
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct MatchingArchetypes
    {
        public Archetype* Archetype;

        public MatchingArchetypes* Next;

        public fixed int IndexInArchetype[1];

        public static int GetAllocationSize(int requiredComponentsCount)
        {
            return sizeof(MatchingArchetypes) + sizeof(int) * (requiredComponentsCount - 1);
        }
    }

    unsafe struct ArchetypeQuery
    {
        public int*     Any;
        public int      AnyCount;
        
        public int*     All;
        public int      AllCount;
        
        public int*     None;
        public int      NoneCount;
    }

    unsafe struct EntityGroupData
    {
        //@TODO: better name or remove entirely...
        public ComponentType*       RequiredComponents;
        public int                  RequiredComponentsCount;
        
        public int*                 ReaderTypes;
        public int                  ReaderTypesCount;

        public int*                 WriterTypes;
        public int                  WriterTypesCount;

        public ArchetypeQuery*      ArchetypeQuery;
        public int                  ArchetypeQueryCount;
        
        public MatchingArchetypes*  FirstMatchingArchetype;
        public MatchingArchetypes*  LastMatchingArchetype;
        
        public EntityGroupData*     PrevGroup;
    }
}
