using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using XLua;

namespace UnityMMO
{
    [Hotfix]
    [LuaCallCSharp]
    public enum SceneObjectType
    {
        None=0,
        Role=1,
        Monster=2,
        NPC=3,
    }
[Hotfix]
[LuaCallCSharp]
public class SceneMgr : MonoBehaviour
{
	public static SceneMgr Instance;
	EntityManager entityManager;
    // public EntityArchetype MainRoleArchetype;
    public EntityArchetype RoleArchetype;
    public EntityArchetype MonsterArchetype;
    public EntityArchetype NPCArchetype;

    Dictionary<long, Entity> entityDic;
    public void Awake()
	{
		Instance = this; // worst singleton ever but it works
		entityManager = World.Active.GetExistingManager<EntityManager>();
        entityDic = new Dictionary<long, Entity>();
	}

	public void InitArcheType()
	{
		// MainRoleArchetype = entityManager.CreateArchetype(
        //         typeof(Position), typeof(PlayerInput),
        //         typeof(MoveSpeed), typeof(SynchPosFlag));
                //typeof(TransformMatrix), 
		RoleArchetype = entityManager.CreateArchetype(
                typeof(Position), typeof(PlayerInput),
                typeof(MoveSpeed), typeof(SynchPosFlag));
	}

	public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddMainRole(long uid)
	{
		Entity role = entityManager.CreateEntity(RoleArchetype);
        entityManager.SetComponentData(role, new Position {Value = new int3(0, 0, 0)});
        entityManager.SetComponentData(role, new MoveSpeed {speed = 12});
        entityManager.AddSharedComponentData(role, GetLookFromPrototype("Prototype/MainRoleRenderPrototype"));
        entityDic.Add(uid, role);
        return role;
	}

    public Entity AddRole(long uid)
	{
		Entity role = entityManager.CreateEntity(RoleArchetype);
        entityManager.SetComponentData(role, new Position {Value = new int3(0, 0, 0)});
        entityManager.SetComponentData(role, new MoveSpeed {speed = 12});
        entityManager.AddSharedComponentData(role, GetLookFromPrototype("Prototype/MainRoleRenderPrototype"));
        entityDic.Add(uid, role);
        return role;
	}

    public Entity AddSceneObject(long uid, SceneObjectType type)
    {
        if (type == SceneObjectType.Role)
            return AddRole(uid);
        return Entity.Null;
    }

    public Entity GetSceneObject(long uid)
    {
        if (entityDic.ContainsKey(uid))
            return entityDic[uid];
        return Entity.Null;
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