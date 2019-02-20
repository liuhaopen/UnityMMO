using Unity.Entities;
using UnityEngine;

//TODO:this solution is temporary
[DisableAutoCreation]
public class UpdateRoleTransformFromLooks : BaseComponentSystem<RoleState>
{
    private ComponentGroup Group;
    
    public UpdateRoleTransformFromLooks(GameWorld world) : base(world) {}

    protected override void Update(Entity entity, RoleState roleState)
    {
        if (roleState.looksEntity == Entity.Null)
            return;

        var looksTrans = EntityManager.GetComponentObject<Transform>(roleState.looksEntity);
        roleState.transform.position = looksTrans.position;
        roleState.transform.rotation = looksTrans.rotation;
    }
}