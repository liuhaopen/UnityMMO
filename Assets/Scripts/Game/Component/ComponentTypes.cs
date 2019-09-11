using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace UnityMMO
{
namespace Component
{
    public struct SceneObjectTypeData : IComponentData
    {
        public SceneObjectType Value;
    }

    //Role类型的话对应RoleID，Monster和NPC类型对应TypeID
    public struct TypeID : IComponentData
    {
        public long Value;
    }

    public struct TimelineState : IComponentData
    {
        public enum NewState
        {
            Allow,//允许新Timeline加进队列
            Forbid,//禁止新Timeline加进队列
        }
        public NewState NewStatus;

        public enum InterruptState
        {
            Allow,//可以播放队列里新的Timeline了
            Forbid,//禁止播放新的Timeline
        }
        public InterruptState InterruptStatus;
    }

    public struct JumpData : IComponentData
    {
        public int JumpCount;
    }

    public struct ActionData : IComponentData
    {
        public int Jump;
        public static ActionData Empty = new ActionData{Jump=0};
    }

    public struct PosOffset : IComponentData
    {
        public float3 Value;
    }

    public struct ActionInfo : IComponentData
    {
        public enum Type
        {
            None = 0,
            Skill1 = 1,
            Skill2 = 2,
            Skill3 = 3,
            Skill4 = 4,
        }
        public Type Value;
    }

    // public struct MoveSpeed : IComponentData
    // {
    //     public float Value;//实际使用的，变动的
    //     public float BaseValue;//基础速度，所有速度加成都以此值为因子
    // }
    
    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }

    public struct PosSynchInfo : IComponentData
    {
        public float3 LastUploadPos;
    }

    public struct LooksInfo : IComponentData
    {
        public enum State
        {
            None = 0,
            Loading = 1,
            Loaded = 2,
        }
        public Entity LooksEntity;
        public State CurState;
        public void Destroy()
        {
            if (CurState == State.Loaded && SceneMgr.Instance.EntityManager.HasComponent<Transform>(LooksEntity))
            {
                var trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(LooksEntity);
                SceneMgr.Instance.World.RequestDespawn(trans.gameObject);
            }
        }
    }

    public struct GroundInfo : IComponentData
    {
        // public Collider groundCollider;
        public Vector3 GroundNormal;
        public float Altitude; 
    }

    public struct HealthStateData : IComponentData      
    {
        public float CurHp;
        public float MaxHp;     
        public int DeathTick;
        public Entity KilledBy;
    }
  
}
}
