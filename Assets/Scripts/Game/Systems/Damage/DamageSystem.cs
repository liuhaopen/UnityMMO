
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityMMO;
using UnityMMO.Component;

[DisableAutoCreation]
public class HandleDamage : BaseComponentSystem
{
    private EntityQuery Group;
    
    public HandleDamage(GameWorld gameWorld) : base(gameWorld)
    {}

    protected override void OnCreate()
    {
        base.OnCreate();
        Group = GetEntityQuery(typeof(HealthStateData));
    }
    
    protected override void OnUpdate()
    {
        var entityArray = Group.ToEntityArray(Allocator.TempJob);
        var healthStateArray = Group.ToComponentDataArray<HealthStateData>(Allocator.TempJob);
        // var collOwnerArray = Group.ToComponentDataArray<HitCollisionOwnerData>();
        for (int i = 0; i < entityArray.Length; i++)
        {
            var healthState = healthStateArray[i];
            
            if (healthState.CurHp <= 0)
                continue;

            var entity = entityArray[i]; 
            // var collOwner = collOwnerArray[i];

            var isDamaged = false;
            var impulseVec = Vector3.zero;
            var damage = 0.0f;
            var damageVec = Vector3.zero;
    
    
            // Apply hitcollider damage events
            var damageBuffer = EntityManager.GetBuffer<DamageEvent>(entity);
            for (var eventIndex=0;eventIndex < damageBuffer.Length; eventIndex++)
            {
                isDamaged = true;
    
                var damageEvent = damageBuffer[eventIndex];
    
                //GameDebug.Log(string.Format("ApplyDamage. Target:{0} Instigator:{1} Dam:{2}", healthState.name, m_world.GetGameObjectFromEntity(damageEvent.instigator), damageEvent.damage ));
                
                // healthState.ApplyDamage(ref damageEvent, (int)Time.time);
                EntityManager.SetComponentData(entity,healthState);
                
                impulseVec += damageEvent.direction * damageEvent.impulse;
                damageVec += damageEvent.direction * damageEvent.damage;
                damage += damageEvent.damage;
    
                //damageHistory.ApplyDamage(ref damageEvent, m_world.worldTime.tick);
    
                // if (damageBuffer[eventIndex].instigator != Entity.Null && EntityManager.Exists(damageEvent.instigator) && EntityManager.HasComponent<DamageHistoryData>(damageEvent.instigator))
                // {
                //     var instigatorDamageHistory = EntityManager.GetComponentData<DamageHistoryData>(damageEvent.instigator);
                //     if (m_world.worldTime.tick > instigatorDamageHistory.inflictedDamage.tick)
                //     {
                //         instigatorDamageHistory.inflictedDamage.tick = m_world.worldTime.tick;
                //         instigatorDamageHistory.inflictedDamage.lethal = 0;
                //     }
                //     if(healthState.health <= 0)
                //         instigatorDamageHistory.inflictedDamage.lethal = 1;
                    
                //     EntityManager.SetComponentData(damageEvent.instigator,instigatorDamageHistory);
                // }
    
                // collOwner.collisionEnabled = healthState.health > 0 ? 1 : 0;
                // EntityManager.SetComponentData(entity,collOwner);
            }
            damageBuffer.Clear();
    
            if (isDamaged)
            {
                var damageImpulse = impulseVec.magnitude;
                var damageDir = damageImpulse > 0 ? impulseVec.normalized : damageVec.normalized;
                
                // var charPredictedState = EntityManager.GetComponentData<CharacterPredictedData>(entity);
                // charPredictedState.damageTick = m_world.worldTime.tick;
                // charPredictedState.damageDirection = damageDir;
                // charPredictedState.damageImpulse = damageImpulse;
                // EntityManager.SetComponentData(entity, charPredictedState);
    
                // if (healthState.health <= 0)
                // {
                //     var ragdollState =  EntityManager.GetComponentData<RagdollStateData>(entity);
                //     ragdollState.ragdollActive = 1;
                //     ragdollState.impulse = impulseVec;
                //     EntityManager.SetComponentData(entity,ragdollState);
                // }
            }
        }
        entityArray.Dispose();
        healthStateArray.Dispose();
    }
}