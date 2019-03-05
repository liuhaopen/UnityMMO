using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.StaticTypeRegistry;

namespace Unity.Entities
{
    public static unsafe class TypeManager
    {
        [AttributeUsage(AttributeTargets.Struct)]
        public class ForcedMemoryOrderingAttribute : Attribute
        {
            public ForcedMemoryOrderingAttribute(ulong ordering)
            {
                MemoryOrdering = ordering;
            }

            public ulong MemoryOrdering;
        }

        [AttributeUsage(AttributeTargets.Struct)]
        public class TypeVersionAttribute : Attribute
        {
            public TypeVersionAttribute(int version)
            {
                TypeVersion = version;
            }

            public int TypeVersion;
        }

        public enum TypeCategory
        {
            ComponentData,
            BufferData,
            ISharedComponentData,
            EntityData,
            Class
        }

        public const int ChunkComponentTypeFlag = 1<<30;
        public const int MaximumTypesCount = 1024 * 10;
        private static volatile int s_Count;
        private static SpinLock s_CreateTypeLock;
        public static int ObjectOffset;

#if !UNITY_CSHARP_TINY
        internal static IEnumerable<TypeInfo> AllTypes { get { return Enumerable.Take(s_Types, s_Count); } }
        private static Dictionary<ulong, int> s_StableTypeHashToTypeIndex;
#endif
        static TypeInfo[] s_Types;

#if !UNITY_ZEROPLAYER
        internal static Type UnityEngineComponentType;

        public static void RegisterUnityEngineComponentType(Type type)
        {
            if (type == null || !type.IsClass || type.IsInterface || type.FullName != "UnityEngine.Component")
                throw new ArgumentException($"{type} must be typeof(UnityEngine.Component).");
            UnityEngineComponentType = type;
        }
#endif
        private struct StaticTypeLookup<T>
        {
            public static int typeIndex;
        }

        public struct EntityOffsetInfo
        {
            public int Offset;
        }

        public struct EqualityHelper<T>
        {
            public delegate bool EqualsFn(T left, T right);
            public delegate int HashFn(T value);

            public static new EqualsFn Equals;
            public static HashFn Hash;
        }

#if !UNITY_CSHARP_TINY
        // https://stackoverflow.com/a/27851610
        static bool IsZeroSizeStruct(Type t)
        {
            return t.IsValueType && !t.IsPrimitive &&
                   t.GetFields((BindingFlags)0x34).All(fi => IsZeroSizeStruct(fi.FieldType));
        }
#endif

        // NOTE: This type will be moved into Unity.Entities.StaticTypeRegistry once Static Type Registry generation is hooked into #!UNITY_CSHARP_TINY builds
        public readonly struct TypeInfo
        {
#if !UNITY_CSHARP_TINY
            public TypeInfo(Type type, int size, TypeCategory category, FastEquality.TypeInfo typeInfo, EntityOffsetInfo[] entityOffsets, ulong memoryOrdering, int bufferCapacity, int elementSize, int alignmentInBytes, ulong stableTypeHash)
            {
                Type = type;
                SizeInChunk = size;
                Category = category;
                FastEqualityTypeInfo = typeInfo;
                EntityOffsetCount = entityOffsets != null ? entityOffsets.Length : 0;
                EntityOffsets = entityOffsets;
                MemoryOrdering = memoryOrdering;
                BufferCapacity = bufferCapacity;
                ElementSize = elementSize;
                AlignmentInBytes = alignmentInBytes;
                IsSystemStateSharedComponent = typeof(ISystemStateSharedComponentData).IsAssignableFrom(type);
                IsSystemStateComponent = typeof(ISystemStateComponentData).IsAssignableFrom(type);
                StableTypeHash = stableTypeHash;
            }

                public readonly Type Type;
                // Note that this includes internal capacity and header overhead for buffers.
                public readonly int SizeInChunk;
                // Normally the same as SizeInChunk (for components), but for buffers means size of an individual element.
                public readonly int ElementSize;
                // Sometimes we need to know not only the size, but the alignment.
                public readonly int AlignmentInBytes;
                public readonly int BufferCapacity;
                public readonly FastEquality.TypeInfo FastEqualityTypeInfo;
                public readonly TypeCategory Category;
                // While this information is available in the Array for EntityOffsets this field allows us to keep Tiny vs non-Tiny code paths the same
                public readonly int EntityOffsetCount;
                public readonly EntityOffsetInfo[] EntityOffsets;
                public readonly ulong MemoryOrdering;
                public readonly ulong StableTypeHash;
                public readonly bool IsSystemStateSharedComponent;
                public readonly bool IsSystemStateComponent;

                public bool IsZeroSized => SizeInChunk == 0;
#else
            public TypeInfo(int typeIndex, TypeCategory category, int entityOffsetCount, int entityOffsetStartIndex, ulong memoryOrdering, ulong stableTypeHash, int bufferCapacity, int typeSize, int elementSize, int alignmentInBytes, bool isSystemStateComponent, bool isSystemStateSharedComponent)
            {
                TypeIndex = typeIndex;
                Category = category;
                EntityOffsetCount = entityOffsetCount;
                EntityOffsetStartIndex = entityOffsetStartIndex;
                MemoryOrdering = memoryOrdering;
                StableTypeHash = stableTypeHash;
                BufferCapacity = bufferCapacity;
                SizeInChunk = typeSize;
                AlignmentInBytes = alignmentInBytes;
                ElementSize = elementSize;
                IsSystemStateComponent = isSystemStateComponent;
                IsSystemStateSharedComponent = isSystemStateSharedComponent;
            }

            public readonly int TypeIndex;
            // Note that this includes internal capacity and header overhead for buffers.
            public readonly int SizeInChunk;
            // Sometimes we need to know not only the size, but the alignment.
            public readonly int AlignmentInBytes;
            // Normally the same as SizeInChunk (for components), but for buffers means size of an individual element.
            public readonly int ElementSize;
            public readonly int BufferCapacity;
            public readonly TypeCategory Category;
            public readonly ulong MemoryOrdering;
            public readonly ulong StableTypeHash;
            public readonly int EntityOffsetCount;
            public readonly int EntityOffsetStartIndex;
            public readonly bool IsSystemStateComponent;
            public readonly bool IsSystemStateSharedComponent;

            public bool IsZeroSized => SizeInChunk == 0;
            public EntityOffsetInfo* EntityOffsets => EntityOffsetCount > 0 ? ((EntityOffsetInfo*) UnsafeUtility.AddressOf(ref StaticTypeRegistry.StaticTypeRegistry.EntityOffsets[0])) + EntityOffsetStartIndex : null;
#endif
        }

        public static unsafe TypeInfo GetTypeInfo(int typeIndex)
        {
            return s_Types[typeIndex & ~ChunkComponentTypeFlag];
        }

        public static TypeInfo GetTypeInfo<T>() where T : struct
        {
            return s_Types[GetTypeIndex<T>()];
        }

        public static Type GetType(int typeIndex)
        {
            #if !UNITY_CSHARP_TINY
                return s_Types[typeIndex & ~ChunkComponentTypeFlag].Type;
            #else
                return StaticTypeRegistry.StaticTypeRegistry.Types[typeIndex & ~ChunkComponentTypeFlag];
            #endif
        }

        public static int GetTypeCount()
        {
            return s_Count;
        }

        public static bool IsSystemStateComponent(int typeIndex) => GetTypeInfo(typeIndex).IsSystemStateComponent;
        public static bool IsSystemStateSharedComponent(int typeIndex) => GetTypeInfo(typeIndex).IsSystemStateSharedComponent;
        public static bool IsSharedComponent(int typeIndex) => TypeCategory.ISharedComponentData == GetTypeInfo(typeIndex).Category;

        // TODO: this creates a dependency on UnityEngine, but makes splitting code in separate assemblies easier. We need to remove it during the biggere refactor.
        private struct ObjectOffsetType
        {
            private void* v0;
            private void* v1;
        }

#if !UNITY_CSHARP_TINY
        private static void AddTypeInfoToTables(TypeInfo typeInfo)
        {

            s_Types[s_Count] = typeInfo;
            s_StableTypeHashToTypeIndex.Add(typeInfo.StableTypeHash, s_Count);
            ++s_Count;
        }
#endif

        public static void Initialize()
        {
            if (s_Types != null)
                return;

            ObjectOffset = UnsafeUtility.SizeOf<ObjectOffsetType>();
            s_CreateTypeLock = new SpinLock();
            s_Types = new TypeInfo[MaximumTypesCount];

            #if !UNITY_CSHARP_TINY
                s_StableTypeHashToTypeIndex = new Dictionary<ulong, int>();
            #endif

            s_Count = 0;

            #if !UNITY_CSHARP_TINY
                AddTypeInfoToTables(new TypeInfo(null, 0, TypeCategory.ComponentData, FastEquality.TypeInfo.Null, null, 0, -1, 0, 1, 0));

                // This must always be first so that Entity is always index 0 in the archetype
                AddTypeInfoToTables(new TypeInfo(typeof(Entity), sizeof(Entity), TypeCategory.EntityData,
                    FastEquality.CreateTypeInfo<Entity>(), EntityRemapUtility.CalculateEntityOffsets<Entity>(), 0, -1, sizeof(Entity), UnsafeUtility.AlignOf<Entity>(), CalculateStableTypeHash(typeof(Entity))));

                InitializeAllIComponentDataTypes();
            #else
                StaticTypeRegistry.StaticTypeRegistry.RegisterStaticTypes();
            #endif
        }

#if UNITY_CSHARP_TINY
        // Called by the StaticTypeRegistry
        internal static void AddStaticTypesFromRegistry(ref TypeInfo[] typeArray, int count)
        {
            if (count >= MaximumTypesCount)
                throw new Exception("More types detected than MaximumTypesCount. Increase the static buffer size.");

            s_Count = 0;
            for (int i = 0; i < count; ++i)
            {
                s_Types[s_Count++] = typeArray[i];
            }
        }
#endif

#if !UNITY_CSHARP_TINY
        static void InitializeAllIComponentDataTypes()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!IsAssemblyReferencingEntities(assembly))
                    continue;

                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IComponentData).IsAssignableFrom(type) && !type.IsAbstract && UnsafeUtility.IsBlittable(type) && type.IsValueType)
                    {
                        GetTypeIndex(type);
                    }
                }
            }
        }

        private static int FindTypeIndex(Type type)
        {
            for (var i = 0; i != s_Count; i++)
            {
                var c = s_Types[i];
                if (c.Type == type)
                    return i;
            }

            return -1;
        }
#else
        private static int FindTypeIndex(Type type)
        {
            for (var i = 0; i != s_Count; i++)
            {
                var c = s_Types[i];
                if (StaticTypeRegistry.StaticTypeRegistry.Types[c.TypeIndex] == type)
                    return i;
            }

            throw new InvalidOperationException("Tried to GetTypeIndex for type that has not been set up by the static type registry.");
        }
#endif

        public static int GetTypeIndex<T>()
        {
            var typeIndex = StaticTypeLookup<T>.typeIndex;
            if (typeIndex != 0)
                return typeIndex;

            typeIndex = GetTypeIndex(typeof(T));
            StaticTypeLookup<T>.typeIndex = typeIndex;
            return typeIndex;
        }

        public static int GetTypeIndex(Type type)
        {
            var index = FindTypeIndex(type);
            return index != -1 ? index : CreateTypeIndexThreadSafe(type);
        }

        public static bool Equals<T>(ref T left, ref T right) where T : struct
        {
            #if !UNITY_CSHARP_TINY
                var typeInfo = TypeManager.GetTypeInfo<T>().FastEqualityTypeInfo;
                return FastEquality.Equals(ref left, ref right, typeInfo);
            #else
                return EqualityHelper<T>.Equals(left, right);
            #endif
        }

        public static bool Equals(void* left, void* right, int typeIndex)
        {
            #if !UNITY_CSHARP_TINY
                var typeInfo = TypeManager.GetTypeInfo(typeIndex).FastEqualityTypeInfo;
                return FastEquality.Equals(left, right, typeInfo);
            #else
                return StaticTypeRegistry.StaticTypeRegistry.Equals(left, right, typeIndex);
            #endif
        }

        public static int GetHashCode<T>(ref T val) where T : struct
        {
            #if !UNITY_CSHARP_TINY
                var typeInfo = TypeManager.GetTypeInfo<T>().FastEqualityTypeInfo;
                return FastEquality.GetHashCode(ref val, typeInfo);
            #else
                return EqualityHelper<T>.Hash(val);
            #endif
        }

        public static int GetHashCode(void* val, int typeIndex)
        {
            #if !UNITY_CSHARP_TINY
                var typeInfo = TypeManager.GetTypeInfo(typeIndex).FastEqualityTypeInfo;
                return FastEquality.GetHashCode(val, typeInfo);
            #else
                return StaticTypeRegistry.StaticTypeRegistry.GetHashCode(val, typeIndex);
            #endif
        }

        public static int GetTypeIndexFromStableTypeHash(ulong stableTypeHash)
        {
#if !UNITY_CSHARP_TINY
            if(s_StableTypeHashToTypeIndex.TryGetValue(stableTypeHash, out var typeIndex))
                return typeIndex;
            return -1;
#else
            throw new InvalidOperationException("Not allowed in Project Tiny");
#endif
        }

#if !UNITY_CSHARP_TINY
        public static bool IsAssemblyReferencingEntities(Assembly assembly)
        {
            const string entitiesAssemblyName = "Unity.Entities";
            if (assembly.GetName().Name.Contains(entitiesAssemblyName))
                return true;

            var referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach(var referenced in referencedAssemblies)
                if (referenced.Name.Contains(entitiesAssemblyName))
                    return true;
            return false;
        }
#endif


#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private static readonly Type[] s_SingularInterfaces =
        {
            typeof(IComponentData),
            typeof(IBufferElementData),
            typeof(ISharedComponentData),
        };

        internal static void CheckComponentType(Type type)
        {
            int typeCount = 0;
            foreach (Type t in s_SingularInterfaces)
            {
                if (t.IsAssignableFrom(type))
                    ++typeCount;
            }

            if (typeCount > 1)
                throw new ArgumentException($"Component {type} can only implement one of IComponentData, ISharedComponentData and IBufferElementData");
        }
#endif

#if !UNITY_CSHARP_TINY
        //
        // The reflection-based type registration path that we can't support with tiny csharp profile.
        // A generics compile-time path is temporarily used (see later in the file) until we have
        // full static type info generation working.
        //
        static ulong CalculateMemoryOrdering(Type type)
        {
            if (type == typeof(Entity))
                return 0;

            var forcedOrdering = type.GetCustomAttribute<ForcedMemoryOrderingAttribute>();
            if (forcedOrdering != null)
                return forcedOrdering.MemoryOrdering;

            var result = CalculateStableTypeHash(type);
            result = result != 0 ? result : 1;
            return result;
        }

        private static int CreateTypeIndexThreadSafe(Type type)
        {
            var lockTaken = false;
            try
            {
                s_CreateTypeLock.Enter(ref lockTaken);

                // After taking the lock, make sure the type hasn't been created
                // after doing the non-atomic FindTypeIndex
                var index = FindTypeIndex(type);
                if (index != -1)
                    return index;

                var componentType = BuildComponentType(type);

                index = s_Count;
                AddTypeInfoToTables(componentType);

                return index;
            }
            finally
            {
                if (lockTaken)
                    s_CreateTypeLock.Exit(true);
            }
        }

        private static ulong HashStringWithFNV1A64(string text)
        {
            // Using http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-1a
            // with basis and prime:
            const ulong offsetBasis = 14695981039346656037;
            const ulong prime = 1099511628211;

            ulong result = offsetBasis;
            foreach (var c in text)
            {
                result = prime * (result ^ (byte)(c & 255));
                result = prime * (result ^ (byte)(c >> 8));
            }
            return result;
        }

        static ulong CalculateStableTypeHash(Type type)
        {
            return HashStringWithFNV1A64(type.AssemblyQualifiedName);
        }

        internal static TypeInfo BuildComponentType(Type type)
        {
            var componentSize = 0;
            TypeCategory category;
            var typeInfo = FastEquality.TypeInfo.Null;
            EntityOffsetInfo[] entityOffsets = null;
            int bufferCapacity = -1;
            var memoryOrdering = CalculateMemoryOrdering(type);
            var stableTypeHash = CalculateStableTypeHash(type);
            int elementSize = 0;
            int alignmentInBytes = 0;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (type.IsInterface)
                throw new ArgumentException($"{type} is an interface. It must be a concrete type.");
#endif
            if (typeof(IComponentData).IsAssignableFrom(type))
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (!type.IsValueType)
                    throw new ArgumentException($"{type} is an IComponentData, and thus must be a struct.");
                if (!UnsafeUtility.IsBlittable(type))
                    throw new ArgumentException(
                        $"{type} is an IComponentData, and thus must be blittable (No managed object is allowed on the struct).");
#endif

                category = TypeCategory.ComponentData;
                if (TypeManager.IsZeroSizeStruct(type))
                    componentSize = 0;
                else
                    componentSize = UnsafeUtility.SizeOf(type);

                typeInfo = FastEquality.CreateTypeInfo(type);
                entityOffsets = EntityRemapUtility.CalculateEntityOffsets(type);

                int sizeInBytes = UnsafeUtility.SizeOf(type);
                // TODO: Implement UnsafeUtility.AlignOf(type)
                alignmentInBytes = 16;
                if(sizeInBytes < 16 && (sizeInBytes & (sizeInBytes-1))==0)
                    alignmentInBytes = sizeInBytes;
            }
            else if (typeof(IBufferElementData).IsAssignableFrom(type))
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (!type.IsValueType)
                    throw new ArgumentException($"{type} is an IBufferElementData, and thus must be a struct.");
                if (!UnsafeUtility.IsBlittable(type))
                    throw new ArgumentException(
                        $"{type} is an IBufferElementData, and thus must be blittable (No managed object is allowed on the struct).");
#endif

                category = TypeCategory.BufferData;
                elementSize = UnsafeUtility.SizeOf(type);

                var capacityAttribute = (InternalBufferCapacityAttribute) type.GetCustomAttribute(typeof(InternalBufferCapacityAttribute));
                if (capacityAttribute != null)
                    bufferCapacity = capacityAttribute.Capacity;
                else
                    bufferCapacity = 128 / elementSize; // Rather than 2*cachelinesize, to make it cross platform deterministic

                componentSize = sizeof(BufferHeader) + bufferCapacity * elementSize;
                typeInfo = FastEquality.CreateTypeInfo(type);
                entityOffsets = EntityRemapUtility.CalculateEntityOffsets(type);

                int sizeInBytes = UnsafeUtility.SizeOf(type);
                // TODO: Implement UnsafeUtility.AlignOf(type)
                alignmentInBytes = 16;
                if(sizeInBytes < 16 && (sizeInBytes & (sizeInBytes-1))==0)
                    alignmentInBytes = sizeInBytes;
             }
            else if (typeof(ISharedComponentData).IsAssignableFrom(type))
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (!type.IsValueType)
                    throw new ArgumentException($"{type} is an ISharedComponentData, and thus must be a struct.");
#endif
                entityOffsets = EntityRemapUtility.CalculateEntityOffsets(type);
                category = TypeCategory.ISharedComponentData;
                typeInfo = FastEquality.CreateTypeInfo(type);
            }
            else if (type.IsClass)
            {
                category = TypeCategory.Class;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (type.FullName == "Unity.Entities.GameObjectEntity")
                    throw new ArgumentException(
                        "GameObjectEntity cannot be used from EntityManager. The component is ignored when creating entities for a GameObject.");
                if (UnityEngineComponentType == null)
                    throw new ArgumentException(
                        $"{type} cannot be used from EntityManager. If it inherits UnityEngine.Component, you must first register TypeManager.UnityEngineComponentType or include the Unity.Entities.Hybrid assembly in your build.");
                if (!UnityEngineComponentType.IsAssignableFrom(type))
                    throw new ArgumentException($"{type} must inherit {UnityEngineComponentType}.");
#endif
            }
            else
            {
                throw new ArgumentException($"{type} is not a valid component.");
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CheckComponentType(type);
#endif
            return new TypeInfo(type, componentSize, category, typeInfo, entityOffsets, memoryOrdering, bufferCapacity, elementSize > 0 ? elementSize : componentSize, alignmentInBytes, stableTypeHash);
        }

        public static int CreateTypeIndexForComponent<T>() where T : struct, IComponentData
        {
            return GetTypeIndex(typeof(T));
        }

        public static int CreateTypeIndexForSharedComponent<T>() where T : struct, ISharedComponentData
        {
            return GetTypeIndex(typeof(T));
        }

        public static int CreateTypeIndexForBufferElement<T>() where T : struct, IBufferElementData
        {
            return GetTypeIndex(typeof(T));
        }

#else
        private static int CreateTypeIndexThreadSafe(Type type)
        {
            throw new InvalidOperationException("Tried to GetTypeIndex for type that has not been set up by the static registry.");
        }
#endif
    }
}
