using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace UnityMMO
{
    public struct PlayerInput : IComponentData
    {
        public float2 Move;
    }

    public struct MoveSpeed : IComponentData
    {
        public float speed;
    }
    

    public struct SynchPosFlag : IComponentData
    {
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

    public struct Health : IComponentData
    {
        public float Value;
    }

    // Pure marker types
    public struct Enemy : IComponentData { }
    public struct EnemyShot : IComponentData { }
    public struct PlayerShot : IComponentData { }

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
