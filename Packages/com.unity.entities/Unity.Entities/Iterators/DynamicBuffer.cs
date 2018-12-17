using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	[StructLayout(LayoutKind.Sequential)]
	[NativeContainer]
    public unsafe struct DynamicBuffer<T> where T : struct
    {
        [NativeDisableUnsafePtrRestriction]
        BufferHeader* m_Buffer;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
	    internal AtomicSafetyHandle m_Safety0;
	    internal AtomicSafetyHandle m_Safety1;
        internal int m_SafetyReadOnlyCount;
        internal int m_SafetyReadWriteCount;
        internal bool m_IsReadOnly;
#endif

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal DynamicBuffer(BufferHeader* header, AtomicSafetyHandle safety, AtomicSafetyHandle arrayInvalidationSafety, bool isReadOnly)
        {
            m_Buffer = header;
            m_Safety0 = safety;
            m_Safety1 = arrayInvalidationSafety;
            m_SafetyReadOnlyCount = isReadOnly ? 2 : 0;
            m_SafetyReadWriteCount = isReadOnly ? 0 : 2;
            m_IsReadOnly = isReadOnly;
        }
#else
        internal DynamicBuffer(BufferHeader* header)
        {
            m_Buffer = header;
        }
#endif

        public int Length
        {
            get
            {
                CheckReadAccess();
                return m_Buffer->Length;
            }
        }

        public int Capacity
        {
            get
            {
                CheckReadAccess();
                return m_Buffer->Capacity;
            }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckBounds(int index)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((uint)index >= (uint)Length)
                throw new IndexOutOfRangeException($"Index {index} is out of range in DynamicBuffer of '{Length}' Length.");
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckReadAccess()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety0);
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety1);
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckWriteAccess()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety0);
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety1);
#endif
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckWriteAccessAndInvalidateArrayAliases()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety0);
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety1);
#endif
        }

        public T this [int index]
        {
            get
            {
                CheckReadAccess();
                CheckBounds(index);
                return UnsafeUtility.ReadArrayElement<T>(BufferHeader.GetElementPointer(m_Buffer), index);
            }
            set
            {
                CheckWriteAccess();
                CheckBounds(index);
                UnsafeUtility.WriteArrayElement<T>(BufferHeader.GetElementPointer(m_Buffer), index, value);
            }
        }

        public void ResizeUninitialized(int length)
        {
            CheckWriteAccessAndInvalidateArrayAliases();
            BufferHeader.EnsureCapacity(m_Buffer, length, UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), BufferHeader.TrashMode.RetainOldData);
            m_Buffer->Length = length;
        }

        public void Clear()
        {
            CheckWriteAccessAndInvalidateArrayAliases();

            m_Buffer->Length = 0;
        }

        public void TrimExcess()
        {
            CheckWriteAccessAndInvalidateArrayAliases();

            byte* oldPtr = m_Buffer->Pointer;
            int length = m_Buffer->Length;

            if (length == Capacity || oldPtr == null)
                return;

            int elemSize = UnsafeUtility.SizeOf<T>();
            int elemAlign = UnsafeUtility.AlignOf<T>();

            byte* newPtr = (byte*) UnsafeUtility.Malloc(elemSize * length, elemAlign, Allocator.Persistent);
            UnsafeUtility.MemCpy(newPtr, oldPtr, elemSize * length);

            m_Buffer->Capacity = length;
            m_Buffer->Pointer = newPtr;

            UnsafeUtility.Free(oldPtr, Allocator.Persistent);
        }

        public void Add(T elem)
        {
            CheckWriteAccess();
            int length = Length;
            ResizeUninitialized(length + 1);
            this[length] = elem;
        }

        public void Insert(int index, T elem)
        {
            CheckWriteAccess();
            int length = Length;
            ResizeUninitialized(length + 1);
            CheckBounds(index); //CheckBounds after ResizeUninitialized since index == length is allowed
            int elemSize = UnsafeUtility.SizeOf<T>();
            byte* basePtr = BufferHeader.GetElementPointer(m_Buffer);
            UnsafeUtility.MemMove(basePtr + (index + 1) * elemSize, basePtr + index * elemSize, elemSize * (length - index));
            this[index] = elem;
        }

        public void AddRange(NativeArray<T> newElems)
        {
            CheckWriteAccess();
            int elemSize = UnsafeUtility.SizeOf<T>();
            int oldLength = Length;
            ResizeUninitialized(oldLength + newElems.Length);

            byte* basePtr = BufferHeader.GetElementPointer(m_Buffer);
            UnsafeUtility.MemCpy(basePtr + oldLength * elemSize, newElems.GetUnsafeReadOnlyPtr<T>(), elemSize * newElems.Length);
        }

        public void RemoveRange(int index, int count)
        {
            CheckWriteAccess();
            CheckBounds(index + count - 1);

            int elemSize = UnsafeUtility.SizeOf<T>();
            byte* basePtr = BufferHeader.GetElementPointer(m_Buffer);

            UnsafeUtility.MemMove(basePtr + index * elemSize, basePtr + (index + count) * elemSize, elemSize * (Length - count - index));

            m_Buffer->Length -= count;
        }

        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        public void* GetUnsafePtr()
        {
            CheckWriteAccess();
            return BufferHeader.GetElementPointer(m_Buffer);
        }

        public DynamicBuffer<U> Reinterpret<U>() where U: struct
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (UnsafeUtility.SizeOf<U>() != UnsafeUtility.SizeOf<T>())
                throw new InvalidOperationException($"Types {typeof(U)} and {typeof(T)} are of different sizes; cannot reinterpret");
#endif
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new DynamicBuffer<U>(m_Buffer, m_Safety0, m_Safety1, m_IsReadOnly);
#else
            return new DynamicBuffer<U>(m_Buffer);
#endif
        }

        /// <summary>
        /// Return a native array that aliases the buffer contents. The array is only legal to access as long as the buffer is not reallocated.
        /// </summary>
        public NativeArray<T> ToNativeArray()
        {
            CheckReadAccess();
            
            var shadow = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(BufferHeader.GetElementPointer(m_Buffer), Length, Allocator.Invalid);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var handle = m_Safety1;
            AtomicSafetyHandle.UseSecondaryVersion(ref handle);          
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref shadow, handle);

#endif
            return shadow;
        }

        public void CopyFrom(NativeArray<T> v)
        {
            ResizeUninitialized(v.Length);
            ToNativeArray().CopyFrom(v);
        }

        public void CopyFrom(T[] v)
        {
            ResizeUninitialized(v.Length);
            ToNativeArray().CopyFrom(v);
        }
    }
}
