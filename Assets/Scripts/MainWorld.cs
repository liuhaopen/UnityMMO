using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

namespace UnityMMO{
public class MainWorld
{
    private MainWorld(){}
    static MainWorld instance = null;
    public EntityManager entityManager;
    public EntityArchetype MainRoleArchetype;

    public static MainWorld GetInstance()
    {
        if (instance == null)
            instance = new MainWorld();
        return instance;
    }

    public void Initialize() {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
        MainRoleArchetype = entityManager.CreateArchetype(
                typeof(Position), typeof(PlayerInput),
                typeof(TransformMatrix), typeof(MovementSpeed));
    }

    public void StartGame() {
        Entity player = entityManager.CreateEntity(MainRoleArchetype);
        entityManager.SetComponentData(player, new Position {Value = new float3(0.0f, 0.0f, 0.0f)});
        entityManager.SetComponentData(player, new MovementSpeed {Value = 12});
        
        entityManager.AddSharedComponentData(player, GetLookFromPrototype("Prototype/MainRoleRenderPrototype"));
    }

    private MeshInstanceRenderer GetLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(proto);
        return result;
    }
}
}