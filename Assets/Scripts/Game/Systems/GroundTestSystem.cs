using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;
using UnityMMO.Component;

[DisableAutoCreation]
public class GroundTestSystem : BaseComponentSystem
{
    EntityQuery Group;

    public GroundTestSystem(GameWorld gameWorld) : base(gameWorld)
    {
        m_defaultLayer = LayerMask.NameToLayer("Default");
        m_playerLayer = LayerMask.NameToLayer("Role");
        m_monsterLayer = LayerMask.NameToLayer("Monster");
        m_npcLayer = LayerMask.NameToLayer("NPC");
        m_platformLayer = LayerMask.NameToLayer("Ground");

        m_mask = 1 << m_defaultLayer | 1 << m_playerLayer | 1 << m_platformLayer | 1 << m_monsterLayer | 1 << m_npcLayer;
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        Group = GetEntityQuery(typeof(Transform), typeof(GroundInfo));
    }

    protected override void OnUpdate()
    {        
        var posArray = Group.ToComponentArray<Transform>();
        var groundArray = Group.ToComponentDataArray<GroundInfo>(Allocator.TempJob);
        var startOffset = 1f;
        var distance = 3f;

        var rayCommands = new NativeArray<RaycastCommand>(posArray.Length, Allocator.TempJob);
        var rayResults = new NativeArray<RaycastHit>(posArray.Length, Allocator.TempJob);

        for (var i = 0; i < posArray.Length; i++)
        {
            // var charPredictedState = charPredictedStateArray[i];
            Vector3 curPos = posArray[i].localPosition;
            var origin = curPos + Vector3.up * startOffset;
            rayCommands[i] = new RaycastCommand(origin, Vector3.down, distance, m_mask);
        }

        var handle = RaycastCommand.ScheduleBatch(rayCommands, rayResults, 10);
        handle.Complete();

        for (var i = 0; i < groundArray.Length; i++)
        {
            var groundInfo = groundArray[i];
            // groundInfo.groundCollider = rayResults[i].collider;
            groundInfo.Altitude = rayResults[i].collider != null ? rayResults[i].distance - startOffset : distance - startOffset;
            // Debug.Log("groundInfo.altitude : "+groundInfo.Altitude+" rayResults[i].collider != null+"+(rayResults[i].collider != null).ToString());

            if (rayResults[i].collider != null)
                groundInfo.GroundNormal = rayResults[i].normal;
        }
        
        rayCommands.Dispose();
        rayResults.Dispose();
        groundArray.Dispose();
    }
    
    readonly int m_defaultLayer;
    readonly int m_playerLayer;
    readonly int m_npcLayer;
    readonly int m_monsterLayer;
    readonly int m_platformLayer;
    readonly int m_mask;
}