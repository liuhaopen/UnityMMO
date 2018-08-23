using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms2D;
using Unity.Transforms;
using XLuaFramework;

namespace UnityMMO
{
    //synch entities property to server
    public class SynchSystem : ComponentSystem
    {
        float lastSynchTime = 0;
        public struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<SynchPosFlag> SynchoFlag;
            
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            //upload per second
            if (Time.time - lastSynchTime < 1)
                return;
            for (int index = 0; index < m_Data.Length; ++index)
            {
                // NetworkManager.SendBytes();
            }         
        }
    }
}