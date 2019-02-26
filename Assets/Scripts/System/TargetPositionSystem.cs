using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using XLuaFramework;
using SprotoType;
using Unity.Collections;

namespace UnityMMO
{
    public class TargetPositionSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            [ReadOnly] public ComponentDataArray<TargetPosition> TargetPos;
            [ReadOnly] public ComponentDataArray<MoveSpeed> MoveSpeed;

            //Excludes entities that contain a PlayerInput from the group
            [ReadOnly] public SubtractiveComponent<PlayerInput> PlayerInputs;
            // public EntityArray Entities;
        }

        [Inject] private Data m_Data;
        
        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;
            var em = PostUpdateCommands;

            for (int index = 0; index < m_Data.Length; ++index)
            {
                float3 forward_dir = m_Data.TargetPos[index].Value - m_Data.Position[index].Value;
                float3 new_pos = forward_dir*(m_Data.MoveSpeed[index].Value*deltaTime);
                em.SetComponent(new Position{Value=new_pos});
            }         
        }
    }
}