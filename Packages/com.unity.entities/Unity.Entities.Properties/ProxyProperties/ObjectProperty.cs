using System.Reflection;
using Unity.Properties;

namespace Unity.Entities.Properties
{
    internal class FieldObjectProperty<TValue> : ValueClassProperty<ObjectContainerProxy, TValue>
    {
        public override bool IsReadOnly => true;

        public FieldInfo Field { get; set; }

        public FieldObjectProperty(FieldInfo info) : base(info.Name, null, null)
        {
            Field = info;
        }

        public override TValue GetValue(ObjectContainerProxy container)
        {
            return (TValue)Field.GetValue(container.o);
        }
    }

    internal class CSharpPropertyObjectProperty<TValue> : ValueClassProperty<ObjectContainerProxy, TValue>
    {
        public override bool IsReadOnly => true;

        public PropertyInfo Property { get; set; }

        public CSharpPropertyObjectProperty(PropertyInfo info) : base(info.Name, null, null)
        {
            Property = info;
        }

        public override TValue GetValue(ObjectContainerProxy container)
        {
            return (TValue) Property.GetValue(container.o);
        }
    }
}
