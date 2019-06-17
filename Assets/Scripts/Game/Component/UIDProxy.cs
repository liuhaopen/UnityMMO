using Unity.Entities;
using UnityEngine;

namespace UnityMMO
{
    public struct UID : IComponentData
    {
        public long Value;
    }

    [DisallowMultipleComponent] 
    public class UIDProxy : ComponentDataProxy<UID> { }

}