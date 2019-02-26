using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;

namespace UnityMMO
{
    [DisableAutoCreation]
    public class PlayerMoveSystem : ComponentSystem
    {
        // public struct Data
        // {
        //     public readonly int Length;
        //     public ComponentDataArray<Position> Position;
        //     // public ComponentDataArray<Heading2D> Heading;
        //     public ComponentDataArray<PlayerInput> Input;
        //     public ComponentDataArray<MoveSpeed> Speed;
        // }
        // [Inject] private Data m_Data;

        public PlayerMoveSystem()
        {
        }
        ComponentGroup group;

        protected override void OnCreateManager()
        {
            Debug.Log("on create player move system");
            base.OnCreateManager();
            group = GetComponentGroup(typeof(Position), typeof(PlayerInput), typeof(MoveSpeed));
        }

        protected override void OnUpdate()
        {
            // var settings = TwoStickBootstrap.Settings;
            var sppeds = group.GetComponentDataArray<MoveSpeed>();
            var playerInput = group.GetComponentDataArray<PlayerInput>();
            var positions = group.GetComponentDataArray<Position>();
            float dt = Time.deltaTime;
            for (int index = 0; index < playerInput.Length; ++index)
            {
                var position = positions[index].Value;
                // var heading = m_Data.Heading[index].Value;
                // var playerInput = m_Data.Input[index];

                // position += dt * playerInput.Move * m_Data.Speed[index].Speed;
                position.x += dt * playerInput[index].Move.x * sppeds[index].Value;
                position.z += dt * playerInput[index].Move.y * sppeds[index].Value;
                // Debug.Log("player move system update position :"+position.ToString());

                positions[index] = new Position {Value = position};
                // m_Data.Heading[index] = new Heading2D {Value = heading};
                // m_Data.Input[index] = playerInput;
            }
        }
    }
}
