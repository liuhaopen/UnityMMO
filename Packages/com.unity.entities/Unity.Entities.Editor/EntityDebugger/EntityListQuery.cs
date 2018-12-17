
using System.Linq;

namespace Unity.Entities.Editor
{
    
    public class EntityListQuery
    {

        public ComponentGroup Group { get; }

        public EntityArchetypeQuery Query { get; }

        public EntityListQuery(ComponentGroup group)
        {
            this.Group = group;
        }

        public EntityListQuery(EntityArchetypeQuery query)
        {
            this.Query = query;
        }
    }

}

