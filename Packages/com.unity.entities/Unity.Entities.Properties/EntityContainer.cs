using System;
using System.Collections.Generic;
using Unity.Properties;

namespace Unity.Entities.Properties
{
    /// <summary>
    /// Container to iterate on Entity instances.
    /// </summary>
    public unsafe struct EntityContainer : IPropertyContainer
    {
        /// <summary>
        /// WARNING This property does NOT implement the List property fully and instead makes the assumption that we are only serializing...
        /// This may cause problems when we start to write UI code and should be looked at.
        /// This is a quick implementation to get higher performance visits
        /// </summary>
        private sealed class ReadOnlyComponentsProperty : StructListStructProperty<EntityContainer, StructProxy>
        {
            public ReadOnlyComponentsProperty(string name) : base(name, null, null) { }

            public override int Count(ref EntityContainer container)
            {
                return container.m_Manager.GetComponentCount(container.m_Entity);
            }

            public override void Accept(ref EntityContainer container, IPropertyVisitor visitor)
            {
                var count = Count(container);

                var listContext = new VisitContext<IList<StructProxy>> { Property = this, Value = null, Index = -1 };

                // @TODO improve, split the deps
                HashSet<Type> primitiveTypes = new HashSet<Type>();

                // try to gather the primitive types for that visitor
                var entityVisitor = visitor as IPrimitivePropertyVisitor;
                if (entityVisitor != null)
                {
                    primitiveTypes = entityVisitor.SupportedPrimitiveTypes();
                }
                else
                {
                    // @TODO remove that dependency
                    // Fallback on the optimized visitor for now
                    primitiveTypes = OptimizedVisitor.SupportedTypes();
                }

                if (visitor.BeginCollection(ref container, listContext))
                {
                    for (var i = 0; i < count; i++)
                    {
                        StructProxy item;
                        if ( ! Get(ref container, i, primitiveTypes, out item))
                        {
                            continue;
                        }

                        var context = new VisitContext<StructProxy>
                        {
                            Property = this,
                            Value = item,
                            Index = i
                        };

                        if (visitor.BeginContainer(ref container, context))
                        {
                            (item.PropertyBag as StructPropertyBag<StructProxy>)?.Visit(ref item, visitor);
                        }

                        visitor.EndContainer(ref container, context);
                    }
                }

                visitor.EndCollection(ref container, listContext);
            }

            private static bool Get(
                ref EntityContainer container,
                int index,
                HashSet<Type> primitiveTypes,
                out StructProxy p)
            {
                var typeIndex = container.m_Manager.GetComponentTypeIndex(container.m_Entity, index);
                var propertyType = TypeManager.GetType(typeIndex);

                p = new StructProxy();

                if (typeof(ISharedComponentData).IsAssignableFrom(propertyType))
                {
                    try
                    {
                        var o = container.m_Manager.GetSharedComponentData(container.m_Entity, typeIndex);

                        // TODO: skip the StructObjectProxyProperty adapter and have the Accept()
                        // TODO:    handle Struct & Object proxies
                        p = new StructProxy
                        {
                            bag = new StructPropertyBag<StructProxy>(
                                new StructObjectProxyProperty(propertyType, o, primitiveTypes)
                                ),
                            data = default(byte*),
                            type = propertyType
                        };

                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                if (typeof(IBufferElementData).IsAssignableFrom(propertyType))
                {
                    IPropertyBag bag = TypeInformation.GetOrCreate(propertyType, primitiveTypes);

                    p = new StructProxy
                    {
                        bag = new StructPropertyBag<StructProxy>(
                                new BufferListProxyProperty(
                                    bag,
                                    propertyType,
                                    container.m_Manager.GetBufferLength(container.m_Entity, typeIndex)
                                    )
                                ),
                        data = (byte*) container.m_Manager.GetBufferRawRW(container.m_Entity, typeIndex),
                        type = propertyType
                    };

                    return true;
                }

                {
                    IPropertyBag bag = TypeInformation.GetOrCreate(propertyType, primitiveTypes);

                    // We skip the property bag & proxy creation for zero sized types (e.g. MonoBehaviors, etc).
                    // They wont get displayed at all in the inspector since we dont know 
                    if (bag.PropertyCount > 1 && ! TypeManager.GetTypeInfo(typeIndex).IsZeroSized)
                    {
                        byte* data = (byte*)container.m_Manager.GetComponentDataRawRW(container.m_Entity, typeIndex);
                        if (data != null)
                        {
                            p = new StructProxy
                            {
                                bag = bag,
                                data = data,
                                type = propertyType
                            };

                            return true;
                        }
                    }
                }

                return false;
            }
        }
        
        private static readonly IListStructProperty<EntityContainer> s_ComponentsProperty = new ReadOnlyComponentsProperty(
            "Components");

        private static readonly StructPropertyBag<EntityContainer> s_PropertyBag = new StructPropertyBag<EntityContainer>(s_ComponentsProperty);

        private readonly EntityManager m_Manager;
        private readonly Entity m_Entity;

        public IVersionStorage VersionStorage => null;
        public IPropertyBag PropertyBag => s_PropertyBag;

        public EntityContainer(EntityManager manager, Entity entity)
        {
            m_Manager = manager;
            m_Entity = entity;
        }
    }
}
