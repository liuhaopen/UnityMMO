using UnityEngine;
using Unity.Entities;

//代表每个角色的逻辑状态
public class RoleState : MonoBehaviour
{
    public int roleUid;
    public string roleName;

    //每个角色都有一个控制的外观Entity负责显示模型，动画等
    public Entity controlledEntity;

}