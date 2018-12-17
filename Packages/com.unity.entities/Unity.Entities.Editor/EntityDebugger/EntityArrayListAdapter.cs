using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.IMGUI.Controls;

namespace Unity.Entities.Editor
{
    internal class EntityArrayListAdapter : IList<TreeViewItem>
    {
        
        private readonly TreeViewItem currentItem = new TreeViewItem();

        private NativeArray<ArchetypeChunk> chunkArray;

        private EntityManager entityManager;

        private ChunkFilter chunkFilter;

        public int Count { get; private set; }

        private Enumerator indexIterator;

        public void SetSource(NativeArray<ArchetypeChunk> newChunkArray, EntityManager newEntityManager, ChunkFilter newChunkFilter)
        {
            chunkArray = newChunkArray;
            chunkFilter = newChunkFilter;
            Count = 0;
            if (chunkArray.IsCreated)
            {
                for (var i = 0; i < chunkArray.Length; ++i)
                {
                    if (chunkFilter == null || (i >= chunkFilter.firstIndex && i <= chunkFilter.lastIndex))
                    {
                        Count += chunkArray[i].Count;
                    }
                }
            }
            entityManager = newEntityManager;
            indexIterator = new Enumerator(this);
        }

        class Enumerator : IEnumerator<TreeViewItem>
        {
            private int linearIndex;
            private int indexInChunk;
            private int currentChunk;

            private readonly EntityArrayListAdapter adapter;

            public Enumerator(EntityArrayListAdapter adapter)
            {
                this.adapter = adapter;
                Reset();
            }
            
            private void UpdateIndexInChunk()
            {
                while (adapter.chunkArray[currentChunk].Count <= indexInChunk)
                    indexInChunk -= adapter.chunkArray[currentChunk++].Count;
            }

            internal void MoveToIndex(int index)
            {
                if (index >= linearIndex)
                {
                    indexInChunk += index - linearIndex;
                    linearIndex = index;
                    UpdateIndexInChunk();
                }
                else
                {
                    Reset(index);
                }
            }
            
            public bool MoveNext()
            {
                ++indexInChunk;
                ++linearIndex;
                
                if (linearIndex >= adapter.Count)
                    return false;

                UpdateIndexInChunk();
                return true;
            }

            private void SetLinearIndex(int index)
            {
                linearIndex = indexInChunk = index;
                currentChunk = 0;
                if (adapter.chunkFilter != null && currentChunk < adapter.chunkFilter.firstIndex)
                    currentChunk = adapter.chunkFilter.firstIndex;
            }

            public void Reset()
            {
                SetLinearIndex(0);
            }

            private void Reset(int index)
            {
                SetLinearIndex(index);
                UpdateIndexInChunk();
            }

            public TreeViewItem Current
            {
                get
                {
                    var entityArray = adapter.chunkArray[currentChunk].GetNativeArray(adapter.entityManager.GetArchetypeChunkEntityType());
                    var entity = entityArray[indexInChunk];
            
                    adapter.currentItem.id = entity.Index;
                    adapter.currentItem.displayName = $"Entity {entity.Index}";
                    return adapter.currentItem;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose() {}
        }

        public TreeViewItem this[int index]
        {
            get
            {
                indexIterator.MoveToIndex(index);
                return indexIterator.Current;
            }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsReadOnly => false;

        public bool GetById(int id, out Entity foundEntity)
        {
            foreach (var chunk in chunkArray)
            {
                var array = chunk.GetNativeArray(entityManager.GetArchetypeChunkEntityType());
                foreach (var entity in array)
                {
                    if (entity.Index == id)
                    {
                        foundEntity = entity;
                        return true;
                    }
                }
            }
            
            foundEntity = Entity.Null;
            
            return false;
        }

        public bool Contains(TreeViewItem item)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerator<TreeViewItem> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TreeViewItem item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(TreeViewItem[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(TreeViewItem item)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(TreeViewItem item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, TreeViewItem item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}