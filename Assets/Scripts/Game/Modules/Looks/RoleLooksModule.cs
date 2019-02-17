using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

public struct RoleLooksSpawnRequest : IComponentData
{
    public int career;
    public Vector3 position;
    public Quaternion rotation;
    public Entity ownerEntity;

    private RoleLooksSpawnRequest(int career, Vector3 position, Quaternion rotation, Entity ownerEntity)
    {
        this.career = career;
        this.position = position;
        this.rotation = rotation;
        this.ownerEntity = ownerEntity;
    }
    
    public static void Create(EntityCommandBuffer commandBuffer, int career, Vector3 position, Quaternion rotation, Entity ownerEntity)
    {
        var data = new RoleLooksSpawnRequest(career, position, rotation, ownerEntity);
        commandBuffer.CreateEntity();
        commandBuffer.AddComponent(data);
    }
}

[DisableAutoCreation]
public class HandleRoleLooksSpawnRequests : BaseComponentSystem
{
    ComponentGroup SpawnGroup;

    public HandleRoleLooksSpawnRequests(GameWorld world) : base(world)
    {
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        SpawnGroup = GetComponentGroup(typeof(RoleLooksSpawnRequest));
    }

    protected override void OnDestroyManager()
    {
        base.OnDestroyManager();
    }

    protected override void OnUpdate()
    {
        var requestArray = SpawnGroup.GetComponentDataArray<RoleLooksSpawnRequest>();
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = SpawnGroup.GetEntityArray();
        
        // Copy requests as spawning will invalidate Group
        var spawnRequests = new RoleLooksSpawnRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++)
        {
            spawnRequests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
        }

        for(var i =0;i<spawnRequests.Length;i++)
        {
            var request = spawnRequests[i];
            var playerState = EntityManager.GetComponentObject<RoleState>(request.ownerEntity);
            var character = SpawnRoleLooks(m_world, playerState, request.position, request.rotation, request.career);
            playerState.controlledEntity = character.gameObject.GetComponent<GameObjectEntity>().Entity; 
        }
    }

    public RoleLooks SpawnRoleLooks(GameWorld world, RoleState owner, Vector3 position, Quaternion rotation, int career)
    {
        return null;
    }

}