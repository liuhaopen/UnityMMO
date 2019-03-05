using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
    public partial class ComponentSystem
    {
        System.Delegate[] m_ForEachCacheDelegate;
        ComponentGroup[]  m_ForEachCacheGroup;
        
        ComponentGroup GetCachedComponentGroup_Delegate(System.Delegate delegateObject)
        {
            // TODO ZERO -- we don't have reflection MethodInfo to compare on the delegate, but maybe
            // we can do something similar in il2cpp
#if !UNITY_ZEROPLAYER
            if (m_ForEachCacheDelegate != null)
            {
                for (int i = 0; i != m_ForEachCacheDelegate.Length; i++)
                {
                    if (m_ForEachCacheDelegate[i] != null && ReferenceEquals(m_ForEachCacheDelegate[i].Method, delegateObject.Method))
                        return m_ForEachCacheGroup[i];
                }
            }
#endif
            return null;
        }

        ComponentGroup CreateCachedComponentGroup_Delegate(System.Delegate delegateObject, params ComponentType[] types)
        {
#if !UNITY_ZEROPLAYER
            // Allocate delegate cache
            if (m_ForEachCacheDelegate == null)
            {
                m_ForEachCacheDelegate = new System.Delegate[4];
                m_ForEachCacheGroup = new ComponentGroup[4];            
            }

            // Find space in cache, if everything is used just replace first element...
            // Nothing smart, for now we just assume we don't have big amount of groups per system...
            int index = 0;
            for (int i = 0; i != m_ForEachCacheGroup.Length; i++)
            {
                if (m_ForEachCacheGroup[i] == null)
                {
                    index = i;
                    break;
                }
            }

            var group = GetComponentGroup(new EntityArchetypeQuery { All = types });
            
            m_ForEachCacheDelegate[index] = delegateObject;
            m_ForEachCacheGroup[index] = group;
            return group;
#else
            return GetComponentGroup(new EntityArchetypeQuery { All = types });
#endif
        }
    }
}
