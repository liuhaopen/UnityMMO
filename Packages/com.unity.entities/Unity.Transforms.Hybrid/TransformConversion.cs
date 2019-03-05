using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

class TransformConversion : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        ForEach((Transform transform) =>
        {
            var entity = GetPrimaryEntity(transform);

            DstEntityManager.AddComponentData(entity, new Position { Value = transform.localPosition });
            DstEntityManager.AddComponentData(entity, new Rotation { Value = transform.localRotation });
            
            if (transform.localScale != Vector3.one)
                DstEntityManager.AddComponentData(entity, new Scale    { Value = transform.localScale });

            if (transform.parent != null)
            {
                var attach = CreateAdditionalEntity(transform);
                DstEntityManager.AddComponentData(attach, new Attach { Parent = GetPrimaryEntity(transform.parent), Child = entity });
            }
        });
    }
}