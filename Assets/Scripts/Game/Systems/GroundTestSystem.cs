using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class GroundTestSystem : BaseComponentSystem
{
    ComponentGroup Group;

    public GroundTestSystem(GameWorld gameWorld) : base(gameWorld)
    {
        m_defaultLayer = LayerMask.NameToLayer("Default");
        m_playerLayer = LayerMask.NameToLayer("collision_player");
        m_platformLayer = LayerMask.NameToLayer("Platform");

        m_mask = 1 << m_defaultLayer | 1 << m_playerLayer | 1 << m_platformLayer;
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetComponentGroup(typeof(Position), typeof(GroundInfo));
    }

    protected override void OnUpdate()
    {        
        var posArray = Group.GetComponentDataArray<Position>();
        var groundArray = Group.GetComponentDataArray<GroundInfo>();
        
        var startOffset = 1f;
        var distance = 3f;

        var rayCommands = new NativeArray<RaycastCommand>(posArray.Length, Allocator.TempJob);
        var rayResults = new NativeArray<RaycastHit>(posArray.Length, Allocator.TempJob);

        for (var i = 0; i < posArray.Length; i++)
        {
            // var charPredictedState = charPredictedStateArray[i];
            Vector3 curPos = posArray[i].Value;
            var origin = curPos + Vector3.up * startOffset;
            rayCommands[i] = new RaycastCommand(origin, Vector3.down, distance, m_mask);
        }

        var handle = RaycastCommand.ScheduleBatch(rayCommands, rayResults, 10);
        handle.Complete();

        for (var i = 0; i < groundArray.Length; i++)
        {
            var groundInfo = groundArray[i];
            // groundInfo.groundCollider = rayResults[i].collider;
            groundInfo.altitude = rayResults[i].collider != null ? rayResults[i].distance - startOffset : distance - startOffset;

            if (rayResults[i].collider != null)
                groundInfo.groundNormal = rayResults[i].normal;
        }
        
        rayCommands.Dispose();
        rayResults.Dispose();
    }
    
    readonly int m_defaultLayer;
    readonly int m_playerLayer;
    readonly int m_platformLayer;
    readonly int m_mask;
}