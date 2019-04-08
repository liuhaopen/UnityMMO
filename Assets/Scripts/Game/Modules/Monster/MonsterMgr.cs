using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO
{
public class MonsterMgr
{
	static MonsterMgr Instance;
    GameWorld m_GameWorld;
    private Transform container;
    public EntityManager EntityManager { get => m_GameWorld.GetEntityManager();}
    public Transform MonsterContainer { get => container; set => container = value; }

    public static MonsterMgr GetInstance()
    {
        if (Instance!=null)
            return Instance;
        Instance = new MonsterMgr();
        return Instance;
    }
    private MonsterMgr(){}

    public void Init(GameWorld world)
	{
        m_GameWorld = world;
        container = GameObject.Find("SceneObjContainer").transform;
	}

    public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddMonster(long uid, Vector3 pos)
	{
        GameObjectEntity monsterGameOE = m_GameWorld.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("Monster"));
        monsterGameOE.name = "Monster_"+uid;
        monsterGameOE.transform.SetParent(container);
        monsterGameOE.transform.localPosition = pos;
        Entity monster = monsterGameOE.Entity;
        InitMonster(monster, uid, pos);
        return monster;
	}

    private void InitMonster(Entity monster, long uid, Vector3 pos)
    {
        EntityManager.AddComponentData(monster, new MoveSpeed {Value = 1000});
        EntityManager.AddComponentData(monster, new TargetPosition {Value = new float3(pos.x, pos.y, pos.z)});
        EntityManager.AddComponentData(monster, new LocomotionState {Value = LocomotionState.State.Idle});
        EntityManager.AddComponentData(monster, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        EntityManager.AddComponentData(monster, new UID {Value=uid});
        // EntityManager.AddComponentData(monster, new JumpState {JumpStatus=JumpState.State.None, JumpCount=0, OriginYPos=0, AscentHeight=0});
        EntityManager.AddComponentData(monster, new PosOffset {Value = float3.zero});
        EntityManager.AddComponentData(monster, new TimelineState {NewStatus=TimelineState.NewState.Allow, InterruptStatus=TimelineState.InterruptState.Allow});
        
        MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(monster);
        rmq.Initialize();
    }
}

}