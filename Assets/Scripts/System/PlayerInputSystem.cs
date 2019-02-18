using Unity.Entities;
using UnityEngine;

namespace UnityMMO
{
    [DisableAutoCreation]
    public class PlayerInputSystem : ComponentSystem
    {
        // struct PlayerData
        // {
        //     public readonly int Length;
        //     public ComponentDataArray<PlayerInput> Input;
        // }
        // [Inject] private PlayerData m_Players;
        public PlayerInputSystem()
        {

        }
        ComponentGroup group;

        protected override void OnCreateManager()
        {
            Debug.Log("on create player input system");
            base.OnCreateManager();
            group = GetComponentGroup(typeof(PlayerInput));
        }

        protected override void OnUpdate()
        {
            // Debug.Log("on OnUpdate player input system");
            float dt = Time.deltaTime;
            var inputDataArray = group.GetComponentDataArray<PlayerInput>();
            for (int i = 0; i < inputDataArray.Length; ++i)
            {
                PlayerInput pi;

                pi.Move.x = Input.GetAxis("Horizontal");
                pi.Move.y = Input.GetAxis("Vertical");
                inputDataArray[i] = pi;
            }
        }
      
    }
}
