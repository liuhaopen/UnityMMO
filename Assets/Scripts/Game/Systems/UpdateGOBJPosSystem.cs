using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Profiling;
using UnityMMO;

[DisableAutoCreation]
public class UpdateGOBJPosSystem : BaseComponentSystem
{
    ComponentGroup Group;

    public UpdateGOBJPosSystem(GameWorld world) : base(world)
    {}

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetComponentGroup(typeof(Position), typeof(Rotation), typeof(Transform));
    }

    protected override void OnUpdate()
    {
        var trans = Group.GetComponentArray<Transform>();
        var pos = Group.GetComponentDataArray<Position>();
        var rotations = Group.GetComponentDataArray<Rotation>();
        Debug.Log("trans.Length : "+trans.Length);
        for (int i = 0; i < trans.Length; i++)
        {
            trans[i].localPosition = pos[i].Value;
            trans[i].localRotation = rotations[i].Value;
        }
    }
}
