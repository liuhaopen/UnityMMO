using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO
{
public class SceneObjectCreator : MonoBehaviour
{
	public static SceneObjectCreator Instance;
	EntityManager entityManager;
    public EntityArchetype MainRoleArchetype;
    public EntityArchetype RoleArchetype;
    public EntityArchetype MonsterArchetype;
    public EntityArchetype NPCArchetype;
    public void Awake()
	{
		Instance = this; // worst singleton ever but it works
		entityManager = World.Active.GetExistingManager<EntityManager>();
	}

	public void InitArcheType()
	{
		MainRoleArchetype = entityManager.CreateArchetype(
                typeof(Position), typeof(PlayerInput),
                typeof(MoveSpeed), typeof(SynchPosFlag));
                //typeof(TransformMatrix), 
		RoleArchetype = entityManager.CreateArchetype(
                typeof(Position), typeof(PlayerInput),
                typeof(MoveSpeed), typeof(SynchPosFlag));
	}

	public void OnDestroy()
	{
		Instance = null;
	}

    public void AddMainRole()
	{
		Entity player = entityManager.CreateEntity(MainRoleArchetype);
        entityManager.SetComponentData(player, new Position {Value = new int3(0, 0, 0)});
        entityManager.SetComponentData(player, new MoveSpeed {speed = 12});
        entityManager.AddSharedComponentData(player, GetLookFromPrototype("Prototype/MainRoleRenderPrototype"));
	}

    public void AddSceneObject(SceneObjectData data)
    {
        var entity = entityManager.CreateEntity(typeof(SceneObjectData));
		entityManager.SetComponentData(entity, data);
    }

	private MeshInstanceRenderer GetLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(proto);
        return result;
    }
}
}}