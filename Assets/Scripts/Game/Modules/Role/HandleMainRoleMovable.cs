// using Unity.Entities;
// using UnityEngine;

// [DisableAutoCreation]
// public class HandleMainRoleMovable : ComponentSystem
// {
//     private ComponentGroup Group;
    
//     public HandleMainRoleMovable(GameWorld world) : base(world) {}

//     protected override void OnCreate()
//  	{
//  		base.OnCreate();
//         Group = GetComponentGroup();
//     }

//     protected override void Update(Entity entity, RoleState roleState, MainRoleTag tag)
//     {
//         if (roleState.looksEntity == Entity.Null)
//             return;

//         var looksTrans = EntityManager.GetComponentObject<Transform>(roleState.looksEntity);
//         roleState.transform.position = looksTrans.position;
//         roleState.transform.rotation = looksTrans.rotation;
//     }
// }