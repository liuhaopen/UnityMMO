using System;
using Unity.Collections.LowLevel.Unsafe;

using Unity.Properties;

namespace Unity.Entities.Properties
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public unsafe class BufferListProxyProperty : ListStructPropertyBase<StructProxy, StructProxy>
    {
        public IPropertyBag ListItemPropertyBag { get; internal set; }

        public Type ListItemComponentType { get; internal set; }

        private int m_SizeOfItem = -1;

        private int m_ItemCount = -1;

        public BufferListProxyProperty(
            IPropertyBag listItemPropertyBag,
            Type listItemComponentType,
            int count)
            : base(listItemComponentType.Name)
        {
            ListItemPropertyBag = listItemPropertyBag;
            ListItemComponentType = listItemComponentType;
            m_ItemCount = count;

            SetUpSizeOfItem();
        }

        private void SetUpSizeOfItem()
        {
            if (m_SizeOfItem != -1)
            {
                return;
            }

            // @TODO a bit convoluted
            var sizeofWrapperListItemType = typeof(SizeOfWrapper<>).MakeGenericType(ListItemComponentType);

            m_SizeOfItem = (int) sizeofWrapperListItemType.GetMethod("SizeOf").Invoke(
                Activator.CreateInstance(sizeofWrapperListItemType), new object[] { }
                );
        }

        private class SizeOfWrapper<T> where T : struct
        {
            public int SizeOf()
            {
                return UnsafeUtility.SizeOf<T>();
            }
        }

        private DynamicBuffer<StructProxy> GetList(ref StructProxy container)
        {
            return new DynamicBuffer<StructProxy>();
        }

        public override int Count(ref StructProxy container)
        {
            return m_ItemCount;
        }

        public override StructProxy GetAt(ref StructProxy container, int index)
        {
            return new StructProxy()
            {
                data = container.data + m_SizeOfItem * index,
                bag = ListItemPropertyBag,
                type = ListItemComponentType
            };
        }

        public override void Accept(ref StructProxy container, IPropertyVisitor visitor)
        {
            var listContext = new VisitContext<StructProxy> { Property = this, Index = -1 };

            if (visitor.BeginCollection(ref container, listContext))
            {
                var itemVisitContext = new VisitContext<StructProxy>
                {
                    Property = this
                };

                var count = Count(container);

                for (var i = 0; i < count; i++)
                {
                    var item = GetAt(ref container, i);

                    itemVisitContext.Value = item;
                    itemVisitContext.Index = i;

                    if (visitor.ExcludeVisit(ref container, itemVisitContext))
                    {
                        continue;
                    }

                    if (visitor.BeginContainer(ref container, itemVisitContext))
                    {
                        PropertyContainer.Visit(ref item, visitor);
                    }
                    visitor.EndContainer(ref container, itemVisitContext);
                }
            }
            visitor.EndCollection(ref container, listContext);
        }

        public override void SetAt(ref StructProxy container, int index, StructProxy item)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override void Add(ref StructProxy container, StructProxy item)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override bool Contains(ref StructProxy container, StructProxy item)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(ref StructProxy container, StructProxy item)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override int IndexOf(ref StructProxy container, StructProxy value)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override void Insert(ref StructProxy container, int index, StructProxy value)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override void RemoveAt(ref StructProxy container, int index)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override void Clear(ref StructProxy container)
        {
            throw new NotImplementedException("List is immutable");
        }

        public override void AddNew(ref StructProxy container)
        {
            throw new NotImplementedException("Cannot add a new item to a ");
        }
    }
}
