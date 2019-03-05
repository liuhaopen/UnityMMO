using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public interface IConvertGameObjectToEntity
{
    void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem converstionSystem);
}

public interface IDeclareReferencedPrefabs
{
    void DeclareReferencedPrefabs(List<GameObject> gameObjects);
}

class ConvertGameObjectToEntitySystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        var convertibles = new List<IConvertGameObjectToEntity>();

        ForEach((Transform transform) =>
        {
            transform.GetComponents(convertibles);

            foreach (var c in convertibles)
            {
                var entity = GetPrimaryEntity((Component) c);
                c.Convert(entity, DstEntityManager, this);
            }
        });
    }
}

class ConvertGameObjectToEntitySystemDeclarePrefabs : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        //@TODO: Support prefab to prefab referencing recursion...
        var declares = new List<IDeclareReferencedPrefabs>();
        var prefabs = new List<GameObject>();

        ForEach((Transform transform) =>
        {
            transform.GetComponents(declares);

            foreach (var c in declares)
                c.DeclareReferencedPrefabs(prefabs);
        });

        foreach (var p in prefabs)
            AddReferencedPrefab(p);    
    }
}