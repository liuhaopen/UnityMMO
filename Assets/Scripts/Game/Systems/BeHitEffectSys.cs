using Unity.Collections;
using Unity.Entities;
using UnityMMO.Component;

namespace UnityMMO
{
    public struct BeHitEffect : IComponentData {
        float EndTime;
        EffectStatus Status;
    }

    [DisableAutoCreation]
    class BeHitEffectSys : BaseComponentSystem
    {
        EntityQuery group;
        
        public BeHitEffectSys(GameWorld world) : base(world) {}
        
        protected override void OnCreate()
        {
            base.OnCreate();
            group = GetEntityQuery(typeof(BeHitEffect), typeof(LooksInfo));
        }

        protected override void OnUpdate()
        {
            var effects = group.ToComponentDataArray<BeHitEffect>(Allocator.TempJob);
            var looksInfos = group.ToComponentDataArray<LooksInfo>(Allocator.TempJob);
            for (var i = 0; i < effects.Length; i++)
            {
                
            }
            effects.Dispose();
            looksInfos.Dispose();
        }
    }
}