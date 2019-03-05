using System;
using System.Collections.Generic;

using Unity.Properties;

namespace Unity.Entities.Properties
{
    internal class StructObjectProxyProperty : ClassValueStructProperty<StructProxy, ObjectContainerProxy>
    {
        public Type ComponentType { get; }

        public IPropertyBag PropertyBag => _bag;

        private readonly ClassPropertyBag<ObjectContainerProxy> _bag;
        private readonly object _wrappedObject;

        public StructObjectProxyProperty(Type t, object o, HashSet<Type> primitiveTypes)
            : base("Shared Component", null, null)
        {
            _wrappedObject = o;

            string displayName = o.GetType().Name;

            _bag = new ClassPropertyBag<ObjectContainerProxy>(
                new ClassObjectProxyProperty(
                    o.GetType(), displayName, o, primitiveTypes));

            ComponentType = t;
        }

        public override ObjectContainerProxy GetValue(ref StructProxy container)
        {
            return new ObjectContainerProxy
            {
                bag = PropertyBag,
                o = _wrappedObject,
            };
        }
    }
}
