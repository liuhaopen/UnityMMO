using Unity.Entities;

namespace UnityMMO
{
    [DisableAutoCreation]
    class EffectSystem : BaseComponentSystem
    {
        EntityQuery Group;
        
        public EffectSystem(GameWorld world) : base(world) {}
        
        protected override void OnCreate()
        {
            base.OnCreate();
            Group = GetEntityQuery(typeof(EffectData));
        }

        protected override void OnUpdate()
        {
            var effects = Group.ToComponentArray<EffectData>();
            for (var i = 0; i < effects.Length; i++)
            {
                
            }
        }
    }
}