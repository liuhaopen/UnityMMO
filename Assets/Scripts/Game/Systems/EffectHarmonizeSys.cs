using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityMMO.Component;
using XLuaFramework;

namespace UnityMMO
{
    [DisableAutoCreation]
    class EffectHarmonizeSys : BaseComponentSystem
    {
        EntityQuery suckHPAndHitGroup;
        
        public EffectHarmonizeSys(GameWorld world) : base(world) {}
        
        protected override void OnCreate()
        {
            base.OnCreate();
            suckHPAndHitGroup = GetEntityQuery(typeof(SuckHPEffect), typeof(BeHitEffect));
        }

        protected override void OnUpdate()
        {
            var suckHPs = suckHPAndHitGroup.ToComponentArray<SuckHPEffect>();
            var beHits = suckHPAndHitGroup.ToComponentArray<BeHitEffect>();
            for (var i = 0; i < suckHPs.Length; i++)
            {
                HandleSuckHPAndHit(suckHPs[i], beHits[i]);
            }
        }

        void HandleSuckHPAndHit(SuckHPEffect suckHP, BeHitEffect beHit)
        {
            //如果同时有吸血和受击，那吸血优先
            if (suckHP.IsShowing() && beHit.IsShowing())
            {
                beHit.Status = EffectStatus.None;
            }
        }
    }
}