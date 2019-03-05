using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace UnityMMO
{
    public struct PlayerInput : IComponentData
    {
        public float2 Move;
    }

    public struct MainRoleTag : IComponentData
    {
    }

    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }
    
    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }

    public struct PosSynchInfo : IComponentData
    {
        public float3 StartPos;
        public float3 EndPos;
    }

    public class GroundInfo : MonoBehaviour
    {
        public Collider groundCollider;
        public Vector3 groundNormal;
        public float altitude; 
        public GroundInfo(){}
    }

    public struct Shot : IComponentData
    {
        public float TimeToLive;
        public float Energy;
    }

    public struct Factions
    {
        public const int kPlayer = 0;
        public const int kEnemy = 1;
    }
    public struct ModifyHealthQueue : IComponentData
    {
        public NativeArray<float> queue;
    }
    public struct Health : IComponentData
    {
        public float Value;
    }
    public struct SceneObjectData : IComponentData
    {
        public enum Type 
        {
            Role,Monster,NPC
        }
        Type type;
    }
    // Pure marker types
    public struct Enemy : IComponentData { }

    public struct EnemyShootState : IComponentData
    {
        public float Cooldown;
    }

    // TODO: Call out that this is better than storing state in the system, because it can support things like replay.
    public struct EnemySpawnCooldown : IComponentData
    {
        public float Value;
    }

    public struct EnemySpawnSystemState : IComponentData
    {
        public int SpawnedEnemyCount;
        public UnityEngine.Random.State RandomState;
    }
}
