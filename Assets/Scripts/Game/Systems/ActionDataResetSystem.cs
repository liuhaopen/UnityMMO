using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class ActionDataResetSystem : BaseComponentSystem
{
    public ActionDataResetSystem(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(ActionData));
    }

    protected override void OnUpdate()
    {
        var datas = group.GetComponentDataArray<ActionData>();
        for (int i=0; i<datas.Length; i++)
        {
            datas[i] = ActionData.Empty;
        }
    }
}