using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
    [NativeContainer]
    [NativeContainerSupportsMinMaxWriteRestriction]
    public unsafe struct BufferArray<T> where T : struct, IBufferElementData
    {
        private ComponentChunkCache m_Cache;
        private ComponentChunkIterator m_Iterator;
        private readonly bool m_IsReadOnly;


        private readonly int m_Length;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly int m_MinIndex;
        private readonly int m_MaxIndex;
        private readonly AtomicSafetyHandle m_Safety0;
        private readonly AtomicSafetyHandle m_ArrayInvalidationSafety;
#pragma warning disable 0414 // assigned but its value is never used
        private int m_SafetyReadOnlyCount;
        private int m_SafetyReadWriteCount;
#pragma warning restore 0414
#endif
        public int Length => m_Length;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal BufferArray(ComponentChunkIterator iterator, int length, bool isReadOnly,
            AtomicSafetyHandle safety, AtomicSafetyHandle arrayInvalidationSafety)
#else
        internal BufferArray(ComponentChunkIterator iterator, int length, bool isReadOnly)
#endif
        {
            m_Length = length;
            m_IsReadOnly = isReadOnly;
            m_Iterator = iterator;
            m_Cache = default(ComponentChunkCache);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_MinIndex = 0;
            m_MaxIndex = length - 1;
            m_Safety0 = safety;
            m_ArrayInvalidationSafety = arrayInvalidationSafety;
            m_SafetyReadOnlyCount = isReadOnly ? 2 : 0;
            m_SafetyReadWriteCount = isReadOnly ? 0 : 2;
#endif
        }

        public DynamicBuffer<T> this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety0);
                if (index < m_MinIndex || index > m_MaxIndex)
                    FailOutOfRangeError(index);
#endif

                if (index < m_Cache.CachedBeginIndex || index >= m_Cache.CachedEndIndex)
                {
                    m_Iterator.MoveToEntityIndexAndUpdateCache(index, out m_Cache, !m_IsReadOnly);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    if (m_Cache.CachedSizeOf < sizeof(BufferHeader))
                        throw new InvalidOperationException("size cache info is broken");
#endif
                }

                BufferHeader* header = (BufferHeader*) ((byte*)m_Cache.CachedPtr + index * m_Cache.CachedSizeOf);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                return new DynamicBuffer<T>(header, m_Safety0, m_ArrayInvalidationSafety, m_IsReadOnly);
#else
                return new DynamicBuffer<T>(header);
#endif
            }
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private void FailOutOfRangeError(int index)
        {
            //@TODO: Make error message utility and share with NativeArray...
            if (index < Length && (m_MinIndex != 0 || m_MaxIndex != Length - 1))
                throw new IndexOutOfRangeException(
                    $"Index {index} is out of restricted IJobParallelFor range [{m_MinIndex}...{m_MaxIndex}] in ReadWriteBuffer.\nReadWriteBuffers are restricted to only read & write the element at the job index. You can use double buffering strategies to avoid race conditions due to reading & writing in parallel to the same elements from a job.");

            throw new IndexOutOfRangeException($"Index {index} is out of range of '{Length}' Length.");
        }
#endif
    }
}
